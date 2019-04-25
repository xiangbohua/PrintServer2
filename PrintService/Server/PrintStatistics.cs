using PrintService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintService.Server
{
    public class PrintStatistics
    {
        public OnPrintStatistics OnPrint = null;

        private int total = 0;
        private int succeed = 0;
        private object _locker = new object();

        public int Total
        {
            get
            {
                return this.total;
            }
        }

        public int Succeed
        {
            get
            {
                return this.succeed;
            }
        }

        public int Errors
        {
            get
            {
                return this.total - this.succeed;
            }
        }

        public void Printed(bool succeed)
        {
            lock (this._locker)
            {
                this.total++;
                if (succeed)
                {
                    this.succeed++;
                }
                try
                {
                    this.OnPrint?.Invoke(this);
                }
                catch
                {
                }
            }
        }
            
    }
}
