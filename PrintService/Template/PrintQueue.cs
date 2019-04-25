using System.Collections.Generic;

namespace PrintService.Template
{
    /// <summary>
    /// Define a queue to achive print serial
    /// </summary>
    public class PrintQueue
    {
        private static Queue<IPrintObject> _printJobs = new Queue<IPrintObject>();
        private static object jobLock = new object();

        /// <summary>
        /// Add a job
        /// </summary>
        /// <param name="m"></param>
        public static void AddJob(IPrintObject m)
        {
            lock (jobLock)
            {
                _printJobs.Enqueue(m);
            }
        }

        /// <summary>
        /// Pop a print job
        /// </summary>
        /// <returns></returns>
        public static IPrintObject PopAJob()
        {
            lock (jobLock)
            {
                if (_printJobs.Count > 0)
                    return _printJobs.Dequeue();
                return null;
            }
        }
    }
}
