using PrintService.UI;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PrintServer2.UI
{
    public partial class Templates : Form
    {
        public Templates()
        {
            InitializeComponent();

            this.Text = Language.Instance().GetText("title_templates", "Supported Templates");
        }

        public void SetList(List<string> templates)
        {            
            foreach (var t in templates)
            {
                this.listTemplates.Items.Add(t);
            }
        }

    }
}
