using PrintServer2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PrintServer2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            new PrintTray();
            Application.Run();
        }


    }
}
