using System;
using HTTPServerLib;

namespace PrintService.Server
{
    public class ConsoleLogger : ILogger
    {
        public void Log(object message)
        {
            Console.WriteLine(message);
        }
    }
}
