using System;
using HTTPServerLib;

namespace PrintServer2.Server
{
    public class ConsoleLogger : ILogger
    {
        public void Log(object message)
        {
            Console.WriteLine(message);
        }
    }
}
