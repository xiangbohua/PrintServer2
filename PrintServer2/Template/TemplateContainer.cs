using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintServer2.Template
{
    public class TemplateContainer
    {
        private static Dictionary<string, Type> container = null;

        /// <summary>
        /// Get template type
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type GetTemplateType(string typeName)
        {
            return container.First(x => x.Key == typeName).Value;
        }

        /// <summary>
        /// Scan all dll in current executing folder to find supported template
        /// </summary>
        public static void SacnTemplate()
        {

        }
    }
}
