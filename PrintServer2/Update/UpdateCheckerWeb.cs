using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PrintServer2.Update
{
    public class UpdateCheckerWeb : IUpdateChecker
    {
        private string updateUrl = null;

        public void BackupFile(UpdateItem item, string backupPath)
        {
            File.Copy(Environment.CurrentDirectory + item.FileName, backupPath);
        }

        public void GetUpdateFIle(UpdateItem item, string savingPath)
        {
            
        }

        public Dictionary<string, string> GetUpdateItems()
        {
            Dictionary<string, string> fileInfoDictionary = new Dictionary<string, string>();
            //try
            //{
            //    List<string> fileVersionInfo = client.GetUpdateFileList(string.Empty).ToList();
            //    foreach (string info in fileVersionInfo)
            //    {
            //        string[] filenameAndVer = info.Split(new char[2] { '%', '%' }, StringSplitOptions.RemoveEmptyEntries);
            //        if (filenameAndVer.Length == 2)
            //            fileInfoDictionary.Add(filenameAndVer[0], filenameAndVer[1]);
            //        else
            //        {
            //            fileInfoDictionary.Add(filenameAndVer[0], "");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

            return fileInfoDictionary;
        }

        public void SetURI(string uri)
        {
            this.updateUrl = uri;
        }
    }
}
