using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PrintService.Utility;

namespace Test
{
    [TestFixture]
    public class TestShareFolder
    {
        [TestCase]
        public void TestConnect()
        {
            var result = ShareFolderHelper.CheckConnectivity("\\\\server\\path", "demo", "demo");

            Assert.AreEqual(result, true);

        }
    }
}
