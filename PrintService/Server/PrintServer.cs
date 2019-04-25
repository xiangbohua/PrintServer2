using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Threading;
using HTTPServerLib;
using PrintService.Common;
using PrintService.Template;
using PrintService.UI;
using PrintService.Utility;

namespace PrintService.Server
{
    public class PrintServer
    {
        private List<string> availablePrinterNames = new List<string>();
        private string selectedPrinterName = "";
        public PrinterLoaded OnPrinterLoaded = null;
        public PrinterChanged OnPrinterChanged = null;
        public PrintServerLogging OnPrintServerLogged = null;
        public StatisticsStateChange OnStatisticsStateChanged = null;

        private IEngin printEngin = null;
        private bool PrintNow = true;

        private bool _requestStop = false;
        private int totalDocument = 0;
        private int succeedDocument = 0;
        private int errorDocument = 0;

        private int port = 4050;

        public int Total => this.totalDocument;
        public int Error => this.errorDocument;

        public int Succeed => this.succeedDocument;
        
        private int GetServerPort()
        {
            var portString = AppSettingHelper.GetOne("ServerPort", this.port.ToString());

            int.TryParse(portString, out this.port);

            return this.port;
        }

        public int GetWorkingPort()
        {
            return this.port;
        }

        public PrintServer()
        {
            string rootPath = Environment.CurrentDirectory;
            this.HttpServer = new HTTPServer("0.0.0.0", this.GetServerPort());
            this.HttpServer.SetRoot(rootPath);
            this.HttpServer.OnPostRequestReceived = this.OnPrintRequestReceived;

            this.HttpServer.Logger = new FileLogger();
            this.printEngin = PrintObjectFactory.GetEngin(AppSettingHelper.GetOne("engin", "PDF"));
            this.printEngin.Initialize();

            this.ServerThread = new Thread(new ThreadStart(HttpServerThread));
            this.OnPrintServerLogged = delegate (string maeesage) { Console.WriteLine(maeesage); };
        }

        public void LoadPrinters()
        {
            this.availablePrinterNames.Clear();
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                this.availablePrinterNames.Add(printer.ToString());
            }
            OnPrinterLoaded?.Invoke(availablePrinterNames);

            string defaultPrinter = ReadPrinterName();
            this.selectedPrinterName = defaultPrinter;
            if (!string.IsNullOrEmpty(defaultPrinter))
            {
                if (this.availablePrinterNames.Contains(defaultPrinter))
                {
                    this.SelectPrinter(defaultPrinter);
                }
                else
                {
                    SafeFireLoging("上次使用的打印机器:" + defaultPrinter + " 已经无效，请重新选择打印机");
                }
            }

            if (this.availablePrinterNames.Count == 0)
            {
                this.SafeFireLoging("打印机加载失败,未找到可用打印机，您可以查看点击查看文件");
            }
            else
            {
                this.SafeFireLoging("打印机加载成功，当前可用打印机数量:" + availablePrinterNames.Count);
            }
        }

        private readonly HTTPServer HttpServer = null;
        private readonly Thread ServerThread = null;
        private void StartServer()
        {
            if (!ServerThread.IsAlive)
            {
                this._requestStop = false;
                ServerThread.Start();
                StartSerialPrintThread();
                SafeFireLoging("服务线程已启动，正在尝试打开服务...");
            }
        }

        public void SetPrintNow(bool printNow)
        {
            this.PrintNow = printNow;
        }

        private void HttpServerThread()
        {
            try
            {
                SafeFireLoging("服务已启动,您现在可以打印...");
                HttpServer.Start();
            }
            catch (Exception ex)
            {
                SafeFireLoging("出现异常，服务已关闭：" + ex.Message);
                HttpServer.Logger.Log(ex.ToString());
            }
        }

        public void StopServer()
        {
            try
            {
                HttpServer.Stop();
                HttpServer?.Stop();
                _requestStop = true;
                this.printEvent.Set();
                SafeFireLoging("服务已停止");
            }
            catch (Exception ex)
            {
                SafeFireLoging("停止服务失败：" + ex.Message);
                HttpServer.Logger.Log(ex.ToString());
            }
        }

        /// <summary>
        /// Return the print engin
        /// </summary>
        /// <returns></returns>
        public IEngin GetEngin()
        {
            return this.printEngin;
        }

        /// <summary>
        /// 获取已经选择的打印机
        /// </summary>
        /// <returns></returns>
        public string GetSelectedPrinterName()
        {
            return this.selectedPrinterName;
        }

        private Thread serialPrinThread = null;
        private void StartSerialPrintThread()
        {
            try
            {
                serialPrinThread = new Thread(new ThreadStart(SerialPrintThread));
                serialPrinThread.Start();

                SafeFireLoging("同步打印线程开启成功，当接请求需要同步打印时系统将请求接受顺序打印！");
            }
            catch (Exception e)
            {
                SafeFireLoging("同步打印线程开启失败，当前不支持顺序打印！");
                HttpServer.Logger.Log(e.ToString());
            }
        }

        private string OnPrintRequestReceived(HttpRequest request, string jsonBody)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(PrintWithThread), request);
                SafeFireLoging(Language.Instance().GetText("add_succeed", "Add task succeed"));

                return Language.Instance().GetText("request_accepted", "Print request acceepted.");
            }
            catch (Exception e)
            {
                HttpServer.Logger.Log(e.ToString());
                return Language.Instance().GetText("request_error", "Print request was not acceepted."); ;
            }
        }
        private object _locker = new object();
        private void PrintWithThread(object objectPara)
        {
            try
            {
                this.totalDocument++;
                HttpRequest httpRequest = objectPara as HttpRequest;
                if (httpRequest != null)
                {
                    IPrintObject model = this.printEngin.GetPrintModel(httpRequest.Body);
                    if (model.Intervel() <= 0)
                    {
                        //不要求顺序打印
                        DoPrintJobWithModel(model);
                    }
                    else
                    {
                        DoPrintJonWithModelSerial(model);
                    }
                }
                else
                {
                    var message = Language.Instance().GetText("type_error", "Current parameter was not acctpeetable");
                    SafeFireLoging(message);
                }
            }
            catch (Exception ex)
            {
                SafeFireLoging(Language.Instance().GetText("err_print", "Error cuer") + ex.Message);
                HttpServer.Logger.Log(ex.ToString());
            }
            SafeFirsStatistics();
        }

        ManualResetEvent printEvent = new ManualResetEvent(false);

        private void SerialPrintThread()
        {
            while (true && !this._requestStop)
            {
                IPrintObject nextModl = PrintQueue.PopAJob();
                if (nextModl == null)
                {
                    printEvent.WaitOne();
                }
                else
                {
                    DoPrintJobWithModel(nextModl);
                    Thread.Sleep(nextModl.Intervel() * 1000);
                }
            }
        }

        public void StopSerialPrint()
        {
            printEvent.Set();
        }

        private void DoPrintJobWithModel(IPrintObject model)
        {
            model.Print(this.PrintNow);            
        }

        private void DoPrintJonWithModelSerial(IPrintObject model)
        {
            PrintQueue.AddJob(model);
            printEvent.Set();
        }

        /// <summary>
        /// 开启流程
        /// </summary>
        public void StartProcess()
        {
            this.LoadPrinters();
            this.StartServer();
        }

        private void SafeFireLoging(string message)
        {
            try
            {
                message = DateTime.Now.ToString() + ": " + message;
                OnPrintServerLogged?.Invoke(message);
            }
            catch (Exception ex)
            {
                HttpServer.Logger.Log(ex.ToString());
            }
        }


        private void SafeFirsStatistics()
        {
            try
            {
                OnStatisticsStateChanged?.Invoke(this.Total, this.Succeed, this.Error);
            }
            catch (Exception ex)
            {
                SafeFireLoging("更新统计信息失败：" + ex.Message);
                HttpServer.Logger.Log(ex.ToString());
            }
        }

        public void SelectPrinter(string printerName)
        {
            try
            {
                if (this.availablePrinterNames.Contains(printerName) && !string.IsNullOrEmpty(printerName))
                {
                    this.selectedPrinterName = printerName;
                    OnPrinterChanged?.Invoke(this.selectedPrinterName);

                    SafeFireLoging("打印机被选中" + printerName + " 已写入配置文件，下次启动将使用此打印机");

                    WritPrinterName(printerName);
                }
                else
                {
                    SafeFireLoging("无效打印机名称");
                }
            }
            catch (Exception ex)
            {
                SafeFireLoging("更新统计信息失败：" + ex.Message);
                HttpServer.Logger.Log(ex.ToString());
            }
        }

        private void PrintFile(string shortFile)
        {
            try
            {
                this.OpenFile(shortFile);
                this.succeedDocument++;
            }
            catch (Exception ex)
            {
                this.SafeFireLoging("打印时出现未知错误，请联系管理员" + ex.Message);
                this.errorDocument++;
                HttpServer.Logger.Log(ex.ToString());
            }

            this.SafeFirsStatistics();
        }

        public void PrintFile(List<string> files)
        {
            foreach (string file in files)
            {
                this.PrintFile(file);
            }
        }


        private string ReadPrinterName()
        {
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == ConfKeyName)
                {
                    return config.AppSettings.Settings[key].Value.ToString();
                }
            }
            return null;
        }

        private const string ConfKeyName = "SelectedPrinterName";

        private void WritPrinterName(string newPrinterName)
        {
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            bool exist = false;
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == ConfKeyName)
                {
                    exist = true;
                }
            }
            if (exist)
            {
                config.AppSettings.Settings.Remove(ConfKeyName);
            }
            config.AppSettings.Settings.Add(ConfKeyName, newPrinterName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void OpenFile(string shortFile)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.Verb = "print";
                Process p = new Process();
                info.FileName = shortFile;
                info.CreateNoWindow = true;

                p.StartInfo.RedirectStandardInput = true;//重定向标准输入  
                p.StartInfo.RedirectStandardOutput = true;//重定向标准输出  
                p.StartInfo.RedirectStandardError = true;//重定向错误输出  

                p.StartInfo.Arguments = this.selectedPrinterName;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo = info;
                p.Start();
                p.WaitForInputIdle();
                //File.Delete(shortFile);
            }
            catch (Win32Exception win32Exception)
            {
                this.SafeFireLoging("打印失败,请安装PDF打开软件并设为默认" + win32Exception.Message);
                throw;
            }

        }

    }
}
