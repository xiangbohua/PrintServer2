using PrintService.Common;
using PrintService.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PrintService.Template
{
    public class TemplateContainer
    {
        public TemplateLoaded TemplateLoaded = null;
        private Dictionary<string, Type> container = null;
        private Dictionary<string, List<TemplateDesc>> templateDescContainer = null;

        public static TemplateContainer instance = null;

        public static TemplateContainer GetInstance()
        {
            if (instance == null)
            {
                instance = new TemplateContainer();
            }
            return instance;
        }

        public Dictionary<string, Type> GetTemplateList()
        {
            return this.container;
        }

        public TemplateContainer()
        {
            this.container = new Dictionary<string, Type>();
            this.templateDescContainer = new Dictionary<string, List<TemplateDesc>>();
        }

        /// <summary>
        /// Get template type
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type GetTemplateType(string typeName)
        {
            if (!this.container.ContainsKey(typeName)) {
                throw new Exception(Language.Instance().GetText("unknown_template", "Unknown template plage check your data."));
            }
            return this.container[typeName];
        }

        /// <summary>
        /// Get the decription of the template
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public List<TemplateDesc> GetTemplateDesc(string templateName)
        {
            if (!this.templateDescContainer.ContainsKey(templateName))
            {
                var templateDesc = new List<TemplateDesc>();
                var properties = this.GetTemplateType(templateName).GetProperties(BindingFlags.Instance & BindingFlags.Public);

                foreach (var p in properties)
                {
                    var attributes = p.GetCustomAttributes(typeof(TemplateParaAttribute), true);
                    if (attributes.Length > 0)
                    {
                        var target = (TemplateParaAttribute)attributes[0];
                        templateDesc.Add(new TemplateDesc()
                        {
                            ParaName = p.Name,
                            ParaType = target.GetParaType(),
                            Demo = target.GetDemo()
                        });
                    }
                }

                this.templateDescContainer.Add(templateName, templateDesc);
                return templateDesc;
            }
            throw new Exception(Language.Instance().GetText("template_desc_error", "Can not found the desc of this template."));
        }


        /// <summary>
        /// Scan all dll in current executing folder to find supported template
        /// </summary>
        public void SacnTemplate()
        {
            var files = Directory.GetFiles(Environment.CurrentDirectory, "*.dll");
            foreach (var fileName in files)
            {
                var foundTemplates = Assembly.LoadFile(fileName).GetTypes().Where<Type>(x => x.IsSubclassOf(typeof(PdfPrintBase)) && x.IsAbstract == false);
                foreach (var template in foundTemplates)
                {
                    var templateName = template.Name;
                    if (!container.ContainsKey(templateName))
                    {
                        container.Add(templateName, template);
                        try
                        {
                            this.TemplateLoaded ?.Invoke(templateName);
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
    }
}
