using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PrintService.Utility;

namespace Test
{
    [TestFixture]
    public class TestShareFolder
    {
        private string testPath = "\\\\192.168.2.47\\TestShare";
        private string tempPath = Environment.CurrentDirectory + "\\temp";

        [SetUp]
        public void Connect()
        {
            ShareFolderHelper.ConnectShareFolder(testPath, "xiangbohua", "xiangbohua1");
            FileHelper.CreateDir(tempPath);            
        }

        [TestCase]
        public void TestListDIr()
        {
            var files = Directory.GetFiles(testPath);

            Assert.AreNotEqual(files.Length, 0);
        }

        [TestCase]
        public void TestCopyFile()
        {
            var files = Directory.GetFiles(testPath);

            Assert.AreNotEqual(files.Length, 0);

            var filePath = files[0];
            var fileName = Path.GetFileName(filePath);

            var newPath = this.tempPath + "\\" + fileName;
            File.Copy(filePath, newPath);

            Assert.True(File.Exists(newPath));
            File.Delete(newPath);
        }

        [TearDown]
        public void Disconnect()
        {
            ShareFolderHelper.Disconnect(testPath);
        }

    }
}
