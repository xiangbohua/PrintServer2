using PrintServer2.Utility;
using System;
using Telerik.Reporting;

namespace PrintServer2.Template
{
    public abstract class PdfPrintBase : Telerik.Reporting.Report, IPrintObject
    {
        public void Print(bool printNow)
        {
            this.SetReportData();
            var filePath = this.GeneratePdf(this);
        }

        public string file_name { get; set; }

        public string print_type { get; set; }

        public int print_interval { get; set; }

        /// <summary>
        /// Let child set their own data 
        /// </summary>
        public abstract void SetReportData();
        
        /// <summary>
        /// Generate pdf file
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        protected string GeneratePdf(IReportDocument report)
        {
            FileHelper.CreateDir(ReportGenerator.FileSavePath);
            string fullPath = ReportGenerator.FileSavePath + "\\" + FileHelper.CleanInvalidFileName(this.file_name) + ".pdf";

            Exception exportException;
            ReportGenerator.SaveReport(report, fullPath, out exportException, "PDF");

            if (exportException != null)
            {
                throw exportException;
            }
            return fullPath;
        }

        public int Intervel()
        {
            return this.print_interval;
        }
    }
}
