using PrintService.Common;
using PrintService.Server;
using PrintService.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PrintServer2.UI
{
    public partial class Logs : Form
    {
        private LogContainer logContainer = null;
        public Logs()
        {
            InitializeComponent();

            this.Text = Language.I.Text("title_log", "Print Logs");
        }

        public void SetLoger(LogContainer logContainer)
        {
            this.logContainer = logContainer;
            this.logContainer.OnLogging = this.OnLogging;
            foreach (var log in logContainer.GetLogs()) {
                this.listTemplates.Items.Add(log);
            }
        }

        private void OnLogging(string logMaeesage)
        {
            if (this.listTemplates.InvokeRequired) {
                this.listTemplates.Invoke(new PrinterChanged(this.OnLogging), logMaeesage);
            }
            else
            {
                this.listTemplates.Items.Add(logMaeesage);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this.logContainer.OnLogging = null;

        }

    }
}
