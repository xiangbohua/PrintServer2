using PrintService.Common;
using PrintService.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PrintService.Update
{
    public class UpdateWorker
    {
        /// <summary>
        /// Process Handler
        /// </summary>
        private OnUpdateProcessed updateProcessedHandler = null;

        /// <summary>
        /// IChecker
        /// </summary>
        private IUpdateChecker updateChecker = null;

        private List<UpdateItem> updateItem = null;

        private const string UpdatePath = "/temp/update";
        private const string BackupPath = "/temp/backup";

        /// <summary>
        /// Indicate that if the update process was needed
        /// </summary>
        private bool needUpdate = false;


        /// <summary>
        /// Simplify construct using lamuda expresion
        /// </summary>
        /// <param name="checker"></param>
        public UpdateWorker(IUpdateChecker checker) => this.updateChecker = checker;

        public void SetUpdateProcessedHandler(OnUpdateProcessed handler)
        {
            this.updateProcessedHandler = handler;
        }

        /// <summary>
        /// Throw the exception when needed
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="step"></param>
        /// <param name="message"></param>
        private void TryThrowException(bool condition, StepEnum step, string message)
        {
            if (!condition) {
                throw new UpdateException(step, message);
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
            this.TryThrowException("" != uri, StepEnum.OnPrepare, "无法获取更新地址");
            this.updateChecker.SetURI(uri);
        }

        /// <summary>
        /// Check update file list
        /// </summary>
        public void CheckUpdateList()
        {
            try
            {
                this.FireEvent(StepEnum.OnCheck, "正在获取更新列表");

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
                this.FireEvent(StepEnum.OnDownload, "下载更新文件失败");
            }

        }

        /// <summary>
        /// Do the backup task
        /// </summary>
        public void BackUp()
        {
            var backupFileFolder = Environment.CurrentDirectory + BackupPath;

            FileHelper.CreateDir(backupFileFolder);
            FileHelper.DeleteFilesInDir(backupFileFolder);

            try
            {
                foreach (var item in this.updateItem)
                {
                    this.updateChecker.BackupFile(item, backupFileFolder);
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
                this.FireEvent(StepEnum.OnDownload, "下载更新文件失败");
            }
        }

        /// <summary>
        /// Do the update work
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {

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
