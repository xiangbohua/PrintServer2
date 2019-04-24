using PrintServer2.UI;
using System;

namespace PrintServer2.Template
{
    /// <summary>
    /// Build model using json string and engin name 
    /// </summary>
    public class PrintObjectFactory
    {
        public static IPrintObject GetPrintModel(string engineName, string printData)
        {
            switch (engineName)
            {
                case "PDF":
                    return PdfEngin.GetPrintModel(printData);
                default:
                    throw new Exception(Language.Instance().GetText("ex_unkown_engin", "Print error unknown engin name!"));
            }

        }

    }
}
