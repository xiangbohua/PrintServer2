using PrintService.Template;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintServer2.UI
{
    public partial class ShowTemplateDesc : Form
    {
        public ShowTemplateDesc()
        {
            InitializeComponent();
        }

        public void SetList(List<TemplateDesc> desc)
        {
            foreach (var d in desc)
            {
                var msg = d.ParaName + " type [" + d.ParaType + "], Demo:" + d.Demo;
                this.listTemplateDesc.Items.Add(msg);
            }
        }

        
    }
}
