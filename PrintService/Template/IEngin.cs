using System.Collections.Generic;

namespace PrintService.Template
{
    public interface IEngin
    {
        IPrintObject GetPrintModel(string printData);

        List<string> GetTemplates();

        void Initialize();

        List<TemplateDesc> GetTemplateDesc(string templateName);
    }
}
