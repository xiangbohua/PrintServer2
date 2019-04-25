using PrintService.Common;
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
        }

        /// <summary>
        /// Get template type
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type GetTemplateType(string typeName)
        {
            return this.container.First(x => x.Key == typeName).Value;
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
