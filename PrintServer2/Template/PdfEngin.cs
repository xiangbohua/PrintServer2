using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrintServer2.UI;
using System;
using System.Web.Script.Serialization;

namespace PrintServer2.Template
{
    /// <summary>
    /// Build print model
    /// </summary>
    public class PdfEngin
    {
        public static IPrintObject GetPrintModel(string modelData)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(modelData);
            string print_type = jo["print_type"].ToString();

            Type modelType = TemplateContainer.GetTemplateType(print_type);
            if (modelType == null)
            {
                throw new Exception(Language.Instance().GetText("err_unknown_template", "Error:Unknown Template"));
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IPrintObject model = (IPrintObject)serializer.Deserialize(modelData, modelType);

            return model;
        }      

    }
}
