using System;
using System.Configuration;
using HTTPServerLib;

namespace PrintService.Server
{
    public class FileLogger : ILogger
    {
        public FileLogger()
        {
            this.log = log4net.LogManager.GetLogger("loginfo");
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase +
                           ConfigurationManager.AppSettings["log4net"];
            var fi = new System.IO.FileInfo(path);
            log4net.Config.XmlConfigurator.Configure(fi);
        }

        private log4net.ILog log = null;

        public void Log(object message)
        {
            log.Info(message.ToString());
        }
    }
}
