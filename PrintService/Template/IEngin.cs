using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintService.Template
{
    public interface IEngin
    {
        IPrintObject GetPrintModel(string printData);

        List<string> GetTemplates();

        void Initialize();
    }
}
