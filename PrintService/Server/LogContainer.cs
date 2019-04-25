using PrintService.Common;
using PrintService.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintService.Server
{
    public class LogContainer
    {
        public PrintServerLogging OnLogging = null;

        private List<string> logs = new List<string>();
        private int maxLogCount = 1000;

        public LogContainer()
        {
            var maxCount = AppSettingHelper.GetOne("MaxLog", this.maxLogCount.ToString());

            int.TryParse(maxCount, out this.maxLogCount);
        }

        /// <summary>
        /// Get the logs
        /// </summary>
        /// <returns></returns>
        public List<string> GetLogs()
        {
            return this.logs;
        }

        /// <summary>
        /// Add one log
        /// </summary>
        /// <param name="log"></param>
        public void AddLog(string log)
        {
            lock (this.logs)
            {
                if (this.logs.Count > this.maxLogCount)
                {
                    this.logs.RemoveAt(0);
                }
                this.logs.Add(log);
                try
                {
                    this.OnLogging?.Invoke(log);
                }
                catch
                {
                }
            }            
        }
    }
}
