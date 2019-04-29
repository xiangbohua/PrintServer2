using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PrintService.Utility
{
    public class ShareFolderHelper
    {
        /// <summary>
        /// Check the connectivity to the remote share folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CheckConnectivity(string path)
        {
            return CheckConnectivity(path, "", "");
        }

        /// <summary>
        /// Check the connectivity to the remote share folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CheckConnectivity(string path, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }


    }
}
