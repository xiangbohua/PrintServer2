using PrintService.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PrintService.Update
{
    public class UpdateTimer
    {
        private UpdateWorker updateWoker = null;
        private Timer updateTimer = null;
        /// <summary>
        /// Update period unit : hour
        /// </summary>
        private decimal updatePeriod = 1;
        public UpdateTimer(UpdateWorker woker)
        {
            this.updateWoker = woker;
            this.updateTimer = new Timer(this.OnUpdate, null, 10000, this.GetUpdatePeriod());
        }

        private int GetUpdatePeriod()
        {
            var periodString = AppSettingHelper.GetOne("UpdatePeriod", this.updatePeriod.ToString());
            decimal.TryParse(periodString, out this.updatePeriod);
            if (this.updatePeriod < (decimal)0.1)
            {
                this.updatePeriod = (decimal)0.1;
            }

            //return (int)(this.updatePeriod * 1000 * 3600);
            return 60000;
        }

        private void OnUpdate(object state)
        {
            this.updateWoker.StartProgress();
        }
    }
}
