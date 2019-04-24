using PrintServer2.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            throw new NotImplementedException();
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
            Application.Exit();
        }
    }
}
