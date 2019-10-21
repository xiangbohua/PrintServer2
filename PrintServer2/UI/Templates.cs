using PrintService.Server;
using PrintService.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PrintServer2.UI
{
    public partial class Templates : Form
    {
        private PrintServer printServer = null;

        public Templates()
        {
            InitializeComponent();

            this.Text = Language.I.Text("title_templates", "Supported Templates");
        }

        public void SetList(PrintServer printServer)
        {
            this.printServer = printServer;
            foreach (var t in this.printServer.GetEngin().GetTemplates())
            {
                this.listTemplates.Items.Add(t);
            }
        }

        private void listTemplates_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var box = (ListBox)sender;
            if (box.SelectedItem != null)
            {
                var tempName = box.SelectedItem.ToString();
                var descObjects = this.printServer.GetEngin().GetTemplateDesc(tempName);

                var form = new ShowTemplateDesc();
                form.SetList(descObjects);
                form.Show();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this.printServer = null;
        }
    }
}
