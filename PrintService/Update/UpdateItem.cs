using System;

namespace PrintService.Update
{
    public class UpdateItem
    {
        public string FileName;

        public UpdateType UpdateType;

        public string Version = "";

        public string FullPath()
        {
            return Environment.CurrentDirectory + "\\" + this.FileName;
        }
    }
}
