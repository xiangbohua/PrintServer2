using PrintService.UI;
using System;

namespace PrintService.Template
{
    /// <summary>
    /// Build model using json string and engin name 
    /// </summary>
    public class PrintObjectFactory
    {
        public static IEngin GetEngin(string enginName)
        {
            switch (enginName)
            {
                case "PDF":
                    return new PdfEngin();
                default:
                    throw new Exception(Language.I.Text("ex_unkown_engin", "Print error unknown engin name!"));
            }
        }
    }
}
