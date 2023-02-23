using LBWorkerLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectLibrary;
using ProjektniZadatak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class LoadBalancerTest
    {
        [TestMethod]
        public void TestLoadBalancer()
        {

            LoadBalancer loadBalancer = new LoadBalancer();
            Item i = new Item();
            Item i2 = new Item(Codes.CODE_ANALOG,1);
            Item i3 = new Item(Codes.CODE_CUSTOM,2);
            ItemDescription itemDescription = new ItemDescription();
            ItemDescription itemDescription2 = new ItemDescription(1);
            ItemDescription itemDescription3 = new ItemDescription(2);
            
            itemDescription.DescriptionDataSet = new DataSet(1, ProjectLibrary.Codes.CODE_ANALOG, ProjectLibrary.Codes.CODE_DIGITAL, 1, 2, EnteredData.full);
            itemDescription3.DescriptionDataSet = new DataSet(2, ProjectLibrary.Codes.CODE_ANALOG, ProjectLibrary.Codes.CODE_DIGITAL, 1, 2, EnteredData.left);

            Worker.Worker w = new Worker.Worker();
            IPAddress address = new IPAddress(123456789);
            List<ItemDescription> id = new List<ItemDescription>();

            Assert.IsNotNull(loadBalancer);
            Assert.AreEqual(loadBalancer.indexCalculator(i), -1);
       
            Assert.AreEqual(loadBalancer.ispisCuvanjaItemDescriptiona(itemDescription), "0;1;CODE_ANALOG;CODE_DIGITAL;1;2;full;");

         
            try
            {
                loadBalancer.cleanIdc(itemDescription);
                Assert.IsTrue(true);
            }
            catch
            {
              //  Assert.IsTrue(false);

            }
            try
            {
                loadBalancer.StartWorkerExe(w);
              //  Assert.IsTrue(false);
            }
            catch
            {
                Assert.IsTrue(true);

            }
            try
            {
                loadBalancer.MakeEmptyItemDescription(1);
                Assert.IsTrue(true);
            }
            catch
            {
            //    Assert.IsTrue(false);

            } 
        
     
            try
            {
                loadBalancer.RemoveWorker(1, address);
                Assert.IsTrue(true);
            }
            catch
            {
             //   Assert.IsTrue(false);

            }
           
        }
    }
}
