using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintServer2.Template
{
    /// <summary>
    /// Define the print object
    /// </summary>
    public interface IPrintObject
    {
        void Print(bool printNows);

        int Intervel();
    }
}
