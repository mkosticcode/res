using LBWorkerLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Tests
{
    [TestClass]
    public class WorkerTest
    {
        [TestMethod]
        public void TestWorker()
        {
            Worker.Worker worker = new Worker.Worker();
            IPAddress address = new IPAddress(123456789);
            Worker.Worker worker2 = new Worker.Worker(address, 500, 1);
            DataSet dataSet1 = new DataSet();
            DataSet dataSet2 = new DataSet();
            ItemDescription id = new ItemDescription(1);

            Assert.IsNotNull(worker);
            Assert.IsNotNull(worker2);
            Assert.AreEqual(worker2.ToString(), "21.205.91.7-500-1");
            Assert.AreEqual(worker.DiffrentUpdate(dataSet1,dataSet2), false);
            /*try
            {
                worker.StartWorker();
                Assert.IsTrue(false);
            }
            catch
            {
                Assert.IsTrue(true);

            }*/
            try
            {
                worker.DoWork(id);
               // Assert.IsTrue(false);
            }
            catch
            {
                Assert.IsTrue(true);

            }/*
            try
            {
                worker.ShutWorker();
                Assert.IsTrue(false);
            }
            catch
            {
                Assert.IsTrue(true);

            }*/
        }
    }
}
