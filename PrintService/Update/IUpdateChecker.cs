using PrintService.UI;
using PrintService.Utility;
using System;
using System.Collections.Generic;

namespace PrintService.Update
{
    public interface IUpdateChecker
    {
        /// <summary>
        /// Set file uri
        /// </summary>
        /// <param name="uri"></param>
        void SetURI(string uri);
        
        /// <summary>
        /// Set verify information
        /// </summary>
        /// <param name="user"></param>
        /// <param name="psd"></param>
        void SetVerify(string user, string psd);

        /// <summary>
        /// List the remote file version
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetUpdateItems();

        /// <summary>
        /// Get file
        /// </summary>
        /// <param name="item"></param>
        /// <param name="savingPath"></param>
        void GetUpdateFIle(UpdateItem item, string savingPath);
    }

    public class UpdateCheckerProvider
    {
        private static Dictionary<string, IUpdateChecker> SupportedChecker = new Dictionary<string, IUpdateChecker>
        {
            { "web", new UpdateCheckerWeb()},
            { "file", new UpdateCheckerFile()},
        };
        public static IUpdateChecker GetChcker()
        {
            var checkerName = AppSettingHelper.GetOne("Updater", "web");

            if (SupportedChecker.ContainsKey(checkerName))
            {
                return SupportedChecker[checkerName];
            }
            var msg = Language.Instance().GetText("known_checker", "Unsupported updater name");
            throw new Exception(msg);
        }

    }
}
