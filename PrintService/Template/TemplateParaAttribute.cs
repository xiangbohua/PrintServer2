using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintService.Template
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TemplateParaAttribute : Attribute
    {
        private string ParaType;
        private string Demo;

        public string GetDemo()
        {
            return this.Demo;
        }

        public string GetParaType()
        {
            return this.ParaType;
        }

        public TemplateParaAttribute(string paraType, string demo)
        {
            this.ParaType = paraType;
            this.Demo = demo;
        }

    }
}
