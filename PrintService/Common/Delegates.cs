using PrintService.Server;
using PrintService.Update;
using System.Collections.Generic;

namespace PrintService.Common
{
    public delegate void VoidStringDelegate(string message);

    public delegate void PrinterChanged(string newPrintName);

    public delegate void PrinterLoaded(List<string> printerNames);

    public delegate void PrintServerLogging(string logMaeesage);
    
    public delegate void OnUpdateProcessed(StepEnum step, string message);

    public delegate void TemplateLoaded(string templateDesc);

    public delegate void OnPrintModeChanged(bool printNow);

    public delegate void OnPrintStatistics(PrintStatistics statistics);
}
