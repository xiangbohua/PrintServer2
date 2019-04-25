using System;
using System.IO;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace PrintService.Template
{
    /// <summary>
    /// Generate pdf file
    /// </summary>
    public class ReportGenerator
    {
        public static string FileSavePath = Environment.CurrentDirectory + "\\PrintFiles";

        public static bool SaveReport(IReportDocument report, string filename, out Exception exception, string format)
        {
            try
            {
                ReportProcessor reportProcessor = new ReportProcessor();
                InstanceReportSource instanceReportSource = new InstanceReportSource();

                instanceReportSource.ReportDocument = report;

                RenderingResult renderingResult = reportProcessor.RenderReport(format.ToUpper(), instanceReportSource, null);
                filename = filename.Replace('\n', ' ');
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(renderingResult.DocumentBytes, 0, renderingResult.DocumentBytes.Length);
                }
                exception = null;
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }
    }
}
