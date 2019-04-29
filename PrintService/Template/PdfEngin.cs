using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrintService.UI;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace PrintService.Template
{
    /// <summary>
    /// Build print model
    /// </summary>
    public class PdfEngin : IEngin
    {
        public IPrintObject GetPrintModel(string modelData)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(modelData);
            string print_type = jo["print_type"].ToString();

            Type modelType = TemplateContainer.GetInstance().GetTemplateType(print_type);
            if (modelType == null)
            {
                throw new Exception(Language.Instance().GetText("err_unknown_template", "Error:Unknown Template"));
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IPrintObject model = (IPrintObject)serializer.Deserialize(modelData, modelType);

            return model;
        }

        public List<TemplateDesc> GetTemplateDesc(string templateName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetTemplates()
        {
            var tList = new List<string>();
            foreach (var tName in TemplateContainer.GetInstance().GetTemplateList().Keys)
            {
                tList.Add(tName);
            }
            return tList;
        }

        public void Initialize()
        {
            TemplateContainer.GetInstance().SacnTemplate();
        }
    }
}
