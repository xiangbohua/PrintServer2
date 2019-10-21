using PrintService.Common;
using PrintService.UI;
using PrintService.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace PrintService.Update
{
    public class UpdateCheckerFile : IUpdateChecker
    {        
        private string updateFolder = null;
        private string user;
        private string psd;
        private string tempCheckPath = Environment.CurrentDirectory + "/temp/checking/";

        public void CleanUpdateServices()
        {
            ShareFolderHelper.Disconnect(this.updateFolder);
        }

        public void GetUpdateFIle(UpdateItem item, string savingPath)
        {
            File.Move(this.tempCheckPath + item.FileName, savingPath + item.FileName);
        }

        public Dictionary<string, string> GetUpdateItems()
        {
            Dictionary<string, string> fileInfoDictionary = new Dictionary<string, string>();
            try
            {
                var files = Directory.GetFiles(updateFolder);
                foreach (var f in files)
                {
                    var fileName = Path.GetFileName(f);
                    File.Copy(updateFolder + fileName, this.tempCheckPath + fileName);
                }
                foreach (string info in files)
                {
                    string[] filenameAndVer = info.Split(new char[2] { '%', '%' }, StringSplitOptions.RemoveEmptyEntries);
                    if (filenameAndVer.Length == 2)
                        fileInfoDictionary.Add(filenameAndVer[0], filenameAndVer[1]);
                    else
                    {
                        fileInfoDictionary.Add(filenameAndVer[0], "");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return fileInfoDictionary;
        }

        public void Prepare()
        {
            try
            {
                ShareFolderHelper.ConnectShareFolder(updateFolder, this.user, this.psd);
            }
            catch (Exception e)
            {
                ThrowHelper.TryThrow(false, StepEnum.OnCheck, Language.I.Text("checking_error", "Can not connect the share folder"));
            }            
        }

        public void SetURI(string uri)
        {
            this.updateFolder = uri;
            if (!this.updateFolder.StartsWith("\\\\"))
            {
                this.updateFolder = "\\\\" + this.updateFolder;
            }

            if (!this.updateFolder.EndsWith("/"))
            {
                this.updateFolder = this.updateFolder + "/";
            }
        }


        public void SetVerify(string user, string psd)
        {
            this.user = user;
            this.psd = psd;
        }
    }
}
