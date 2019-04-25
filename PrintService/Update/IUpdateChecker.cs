using System.Collections.Generic;

namespace PrintService.Update
{
    public interface IUpdateChecker
    {
        void SetURI(string uri);

        Dictionary<string, string> GetUpdateItems();
        
        void GetUpdateFIle(UpdateItem item, string savingPath);

        void BackupFile(UpdateItem item, string backupPath);
    }
}
