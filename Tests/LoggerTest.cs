using System;
using System.Collections.Generic;
using LBWorkerLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectLibrary;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void TestCollectionDescription()
        {

            Logger.Logger logger1 = new Logger.Logger();
            Assert.IsNotNull(logger1);
            Assert.AreEqual(logger1.Log("aaa"),0);
            Assert.AreEqual(Logger.Program.Log("aaa"),0);

        }
    }
}
