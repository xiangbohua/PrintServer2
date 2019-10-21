using PrintService.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintService.Common
{
    public class ThrowHelper
    {
        /// <summary>
        /// Throw the exception when needed
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="step"></param>
        /// <param name="message"></param>
        public static void TryThrow(bool condition, StepEnum step, string message)
        {
            if (!condition)
            {
                throw new UpdateException(step, message);
            }
        }
    }
}
