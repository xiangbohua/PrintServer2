using PrintService.Common;
using PrintService.UI;
using PrintService.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PrintService.Update
{
    public class UpdateWorker
    {
        public VoidStringDelegate OnUpdateStarted = null;

        private UpdateTimer  timer = null;
        /// <summary>
        /// Process Handler
        /// </summary>
        private OnUpdateProcessed updateProcessedHandler = null;

        /// <summary>
        /// IChecker
        /// </summary>
        private IUpdateChecker updateChecker = null;

        private List<UpdateItem> updateItem = null;

        private const string UpdatePath = "/temp/update/";
        private const string BackupPath = "/temp/backup/";

        /// <summary>
        /// Indicate that if the update process was needed
        /// </summary>
        private bool needUpdate = false;

        private bool inProgress = false;

        /// <summary>
        /// Simplify construct using lamuda expresion
        /// </summary>
        /// <param name="checker"></param>
        public UpdateWorker(IUpdateChecker checker) {
            this.updateChecker = checker;
            this.timer = new UpdateTimer(this);
        }

        public void SetUpdateProcessedHandler(OnUpdateProcessed handler)
        {
            this.updateProcessedHandler = handler;
        }

        /// <summary>
        /// Start update progress
        /// </summary>
        public void StartProgress()
        {
            if (this.inProgress)
            {
                return;
            }

            this.inProgress = true;

            try
            {
                this.Prepare();
                this.CheckUpdateList();
                if (this.needUpdate)
                {
                    var msg = Language.I.Text("update_started", "Update progress started.");

                    this.OnUpdateStarted?.Invoke(msg);
                } 
                this.BackUp();
                this.DownloadFile();
            }
            catch
            {
                this.CleanUpOnError();
            }
            finally
            {
                this.inProgress = false;
            }
        }

        

        /// <summary>
        /// Fire the update event
        /// </summary>
        /// <param name="step"></param>
        /// <param name="message"></param>
        private void FireEvent(StepEnum step, string message)
        {
            this.updateProcessedHandler?.Invoke(step, message);
        }

        /// <summary>
        /// Check current state
        /// </summary>
        /// <returns></returns>
        public bool NeedUpdate()
        {
            return this.needUpdate;
        }


        public void Prepare()
        {
            var uri = AppSettingHelper.GetOne("UpdateServer");
            ThrowHelper.TryThrow("" != uri, StepEnum.OnPrepare, Language.I.Text("empty_update_uri", "Can not get update url!"));
            this.updateChecker.SetURI(uri);
        }

        /// <summary>
        /// Check update file list
        /// </summary>
        public void CheckUpdateList()
        {
            try
            {
                this.FireEvent(StepEnum.OnCheck, Language.I.Text("getting_list", "Fetching update file list!"));

                var cp = new VersionComparer();
                this.updateItem = cp.CompareVersion(this.GetLocalFileVersion(), this.updateChecker.GetUpdateItems());
                
                this.needUpdate = this.updateItem.Count > 0;
                this.FireEvent(StepEnum.OnCheck, "关系列表获取完成,共" + this.updateItem.Count + "个文件需要更新");
            }
            catch (UpdateException e)
            {
                this.needUpdate = false;
                this.FireEvent(StepEnum.OnCheck, e.Message);
                throw;
            }
            catch (Exception ex)
            {
                this.needUpdate = false;
                this.FireEvent(StepEnum.OnCheck, "获取更新文件列表失败");
            }
        }

        /// <summary>
        /// Get the local file info
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetLocalFileVersion()
        {
            var localFileVerInfos = new Dictionary<string, string>();

            var curruentDir = Environment.CurrentDirectory + "\\";
            DirectoryInfo dirInfo = new DirectoryInfo(curruentDir);

            foreach (FileInfo file in dirInfo.GetFiles("*dll"))
            {
                localFileVerInfos.Add(file.Name, FileVersionInfo.GetVersionInfo(file.FullName).FileVersion);
            }

            return localFileVerInfos;
        }
        
        /// <summary>
        /// Start download the updating file
        /// </summary>
        public void DownloadFile()
        {
            if (!this.needUpdate)
            {
                return;
            }
            var updateFileFolder = Environment.CurrentDirectory + UpdatePath;

            FileHelper.CreateDir(updateFileFolder);
            FileHelper.DeleteFilesInDir(updateFileFolder);

            try {
                foreach (var item in this.updateItem)
                {
                    this.updateChecker.GetUpdateFIle(item, updateFileFolder);
                }
            }
            catch (UpdateException e)
            {
                this.needUpdate = false;
                this.FireEvent(e.GetStep(), e.Message);
                throw;
            }
            catch (Exception ex)
            {
                this.needUpdate = false;
                var msg = Language.I.Text("download_error", "Someting wrony when downloading update files");
                this.FireEvent(StepEnum.OnDownload, msg);
            }

        }

        /// <summary>
        /// Do the backup task
        /// </summary>
        public void BackUp()
        {
            if (!this.needUpdate)
            {
                return;
            }
            var backupFileFolder = Environment.CurrentDirectory + BackupPath;

            FileHelper.CreateDir(backupFileFolder);
            FileHelper.DeleteFilesInDir(backupFileFolder);

            try
            {
                foreach (var item in this.updateItem)
                {
                    File.Copy(item.FullPath(), backupFileFolder);
                }
            }
            catch (UpdateException e)
            {
                this.needUpdate = false;
                this.FireEvent(e.GetStep(), e.Message);
                throw;
            }
            catch (Exception ex)
            {
                this.needUpdate = false;
                var msg = Language.I.Text("backup_error", "Someting wrony when backing up update files");
                this.FireEvent(StepEnum.OnBackup, msg);
            }
        }

        /// <summary>
        /// Do the update work
        /// Kill this 
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            if (!this.needUpdate)
            {
                return;
            }
        }

        /// <summary>
        /// Clean files when update error
        /// </summary>
        private void CleanUpOnError()
        {
            var updateFileFolder = Environment.CurrentDirectory + UpdatePath;
            FileHelper.DeleteFilesInDir(updateFileFolder);
            var backupFileFolder = Environment.CurrentDirectory + BackupPath;
            FileHelper.DeleteFilesInDir(backupFileFolder);
        }


    }

}
