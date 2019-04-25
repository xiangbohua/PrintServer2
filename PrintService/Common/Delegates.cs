using PrintService.Update;
using System.Collections.Generic;

namespace PrintService.Common
{
    public delegate void PrinterChanged(string newPrintName);

    public delegate void PrinterLoaded(List<string> printerNames);

    public delegate void PrintServerLogging(string logMaeesage);

    public delegate void StatisticsStateChange(int total, int succeed, int errpr);

    public delegate void OnUpdateProcessed(StepEnum step, string message);

    public delegate void TemplateLoaded(string templateDesc);
}
