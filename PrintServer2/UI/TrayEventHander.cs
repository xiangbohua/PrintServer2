using PrintService.Server;
using System;
using System.Windows.Forms;

namespace PrintServer2.UI
{
    internal class TrayEventHander
    {
        private PrintServer printServer = null;
        /// <summary>
        /// Let the hanlder hold a server object and then all those click event will be processed in this class
        /// </summary>
        /// <param name="printServer"></param>
        public TrayEventHander(PrintServer server)
        {
            this.printServer = server;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RefreshPrinter_Click(object sender, EventArgs e)
        {
            this.printServer.LoadPrinters();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectPrinter_Click(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            this.printServer.SelectPrinter(menuItem.Tag.ToString());
        }

        /// <summary>
        /// when exit menu clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Exit_Click(object sender, EventArgs e)
        {
            this.printServer.StopServer();
            Application.Exit();
        }

        internal void SupportedTemplates_Click(object sender, EventArgs e)
        {
            Templates formTemplate = new Templates();
            formTemplate.SetList(this.printServer);
            formTemplate.Show();
        }

        internal void PrintMode_Click(object sender, EventArgs e)
        {
            var printNow = (MenuItem)sender;

            this.printServer.SetPrintNow(!printNow.Checked);
        }

        internal void ShowLog_Click(object sender, EventArgs e)
        {
            Logs logForm = new Logs();
            logForm.SetLoger(this.printServer.GetLoger());
            logForm.Show();
        }
    }
}
