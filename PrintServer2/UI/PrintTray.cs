using PrintServer2.Properties;
using PrintService.Server;
using PrintService.UI;
using PrintService.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PrintServer2.UI
{
    public class PrintTray
    {
        private NotifyIcon notifyIcon = null;
        private Language language = null;
        private PrintServer printServer = null;

        private TrayEventHander clickHander = null;

        private MenuItem exitItem = null;
        private MenuItem selectPrinter = null;
        private MenuItem printNow = null;
        private MenuItem showLog = null;
        private MenuItem changeLanguage = null;
        private MenuItem changePort = null;
        private MenuItem supportedTemplate = null;

        public PrintTray()
        {            
            this.InitServer();

            this.InitEventHalder();

            this.InitTray();

            this.printServer.StartProcess();
        }

        /// <summary>
        /// Init the event handler
        /// </summary>
        private void InitEventHalder()
        {
            this.clickHander = new TrayEventHander(this.printServer);
        }

        /// <summary>
        /// Init the tray object
        /// </summary>
        private void InitTray()
        {
            this.language = Language.Instance();

            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = AppSettingHelper.GetOne("IconHint", "Print server was runing in backgroud");
            this.notifyIcon.Text = AppSettingHelper.GetOne("HideHint", "Click icon to open main window");

            this.notifyIcon.Icon = this.GetTaskBarIcon();
            this.notifyIcon.Visible = true;
            this.notifyIcon.ShowBalloonTip(2000);
            
            this.changeLanguage = new MenuItem(this.language.GetText("language", "Lanaguage"));

            var portString = this.language.GetText("language", "Lanaguage") + "(" + this.printServer.GetWorkingPort().ToString() + ")";
            this.changePort = new MenuItem();

            this.supportedTemplate = new MenuItem(this.language.GetText("show_templates", "Show Temlpates"));
            this.supportedTemplate.Click += this.clickHander.SupportedTemplates_Click;


            this.selectPrinter = new MenuItem(this.language.GetText("select_printer", "Select Printer"));
            
            this.printNow = new MenuItem(this.language.GetText("print_now", "Print Now"));
            this.printNow.Checked = this.printServer.GetPrintMode();
            this.printNow.Click += this.clickHander.PrintMode_Click;


            this.showLog = new MenuItem(this.language.GetText("show_log", "Show Logs"));
            this.showLog.Click += this.clickHander.ShowLog_Click;

            this.exitItem = new MenuItem(this.language.GetText("exit", "Exit"));
            this.exitItem.Click += this.clickHander.Exit_Click;

            MenuItem[] childen = new MenuItem[] {
                this.changeLanguage,
                this.selectPrinter,
                this.supportedTemplate,
                this.showLog,
                this.printNow,
                this.exitItem
            };
            notifyIcon.ContextMenu = new ContextMenu(childen);
        }

        /// <summary>
        /// Try to get icon for task bar
        /// first: try to find a icon file named logo.icon in folder Resources. if not found then use the build-in icon
        /// </summary>
        /// <returns></returns>
        private Icon GetTaskBarIcon()
        {
            var filePath = Environment.CurrentDirectory + "\\logo.ico";
            var logo = File.Exists(filePath);
            if (logo)
            {
                return new Icon(filePath);
            }
            return Resources._default;
        }

        /// <summary>
        /// Init the print server
        /// </summary>
        private void InitServer()
        {
            this.printServer = new PrintServer();
            this.printServer.OnPrinterLoaded = this.OnPrinterLoaded;
            this.printServer.OnPrinterChanged = this.OnPrinterSelected;
            this.printServer.OnPrintModeChanged = this.OnPrintModeChanded;
        }

        private void OnPrintModeChanded(bool printNow)
        {
            this.printNow.Checked = printNow;
        }

        /// <summary>
        /// When the print server has loaded all printer names then call this method to add meun items
        /// </summary>
        /// <param name="printerNames"></param>
        private void OnPrinterLoaded(List<string> printerNames)
        {
            this.selectPrinter.MenuItems.Clear();
            
            var refreshPrinter = new MenuItem(this.language.GetText("refresh_printer", "Refresh Printer"));
            refreshPrinter.Click += this.clickHander.RefreshPrinter_Click;
            this.selectPrinter.MenuItems.Add(refreshPrinter);
            foreach (var pName in printerNames) {
                var printerMenu = new MenuItem(pName);
                printerMenu.Tag = pName;
                printerMenu.Click += this.clickHander.SelectPrinter_Click;
                

                this.selectPrinter.MenuItems.Add(printerMenu);
            }
        }

        /// <summary>
        /// Triggered when print selected
        /// </summary>
        /// <param name="printName"></param>
        private void OnPrinterSelected(string printName)
        {
            foreach (var mi in this.selectPrinter.MenuItems) {
                if (mi is MenuItem)
                {
                    var it = mi as MenuItem;
                    if (it.Tag != null && it.Tag.ToString() == this.printServer.GetSelectedPrinterName())
                    {
                        it.Checked = true;
                    }
                    else
                    {
                        it.Checked = false;
                    }
                }
                
            }            
        }

        /// <summary>
        /// Change language name
        /// </summary>
        /// <param name="languageName"></param>
        private void ChangeLanguage(string languageName)
        {
            this.language.ChangLanguage(languageName);
        }        
    }
}
