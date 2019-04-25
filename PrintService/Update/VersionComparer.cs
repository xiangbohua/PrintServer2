using System;
using System.Collections.Generic;

namespace PrintService.Update
{
    public class VersionComparer
    {
        public List<UpdateItem> CompareVersion(Dictionary<string, string> localInfo, Dictionary<string, string> serverInfo)
        {
            var result = new List<UpdateItem>();

            //检查远端文件，如果本地没有表示是新的文件，
            foreach (KeyValuePair<string, string> remoteFileVersion in serverInfo)
            {
                if (!localInfo.ContainsKey(remoteFileVersion.Key))
                {
                    //远端有，本地没有：新增文件
                    var upItem = new UpdateItem() { FileName = remoteFileVersion.Key, UpdateType = UpdateType.NewFile, Version = remoteFileVersion.Value };
                    result.Add(upItem);
                }
                else
                {
                    //远端文件在本地有，比较版本
                    if (this.CheckFileVersion(localInfo[remoteFileVersion.Key], remoteFileVersion.Value))
                    {
                        var upItem = new UpdateItem() { FileName = remoteFileVersion.Key, UpdateType = UpdateType.NewVersion, Version = remoteFileVersion.Value };
                        result.Add(upItem);
                    }
                }
            }

            //比较本地文件是否在远端文件中出现，没有则视为需要删除的文件
            foreach (KeyValuePair<string, string> localFileVersion in localInfo)
            {
                if (!serverInfo.ContainsKey(localFileVersion.Key))
                {
                    //远端文件列表中未出现本地文件
                    var upItem = new UpdateItem() { FileName = localFileVersion.Key, UpdateType = UpdateType.DeleteFile, Version = localFileVersion.Value };
                    result.Add(upItem);
                }
            }

            return result;
        }

        /// <summary>
        /// 文件是否需要更新
        /// </summary>
        /// <param name="localV">本地版本</param>
        /// <param name="remoteV">远端版本</param>
        /// <returns>true 需要更新</returns>
        /// <returns>false 不需要更新</returns>
        protected bool CheckFileVersion(string localV, string remoteV)
        {
            if (string.IsNullOrEmpty(localV) || string.IsNullOrEmpty(remoteV))
                return true;

            Version local = Version.Parse(localV);
            Version remote = Version.Parse(remoteV);

            return remote.CompareTo(local) > 0;
        }
    }
}
