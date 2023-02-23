using LBWorkerLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ItemDescriptionTest
    {
        [TestMethod]
        public void TestItemDescription()
        {

            ItemDescription itemDescription1 = new ItemDescription(1);
            ItemDescription itemDescription = new ItemDescription();
            itemDescription.ItemsList = new List<ProjectLibrary.Item>();
            itemDescription.DescriptionDataSet = new DataSet(1,ProjectLibrary.Codes.CODE_ANALOG, ProjectLibrary.Codes.CODE_DIGITAL,1,2,EnteredData.full);

            Assert.IsNotNull(itemDescription);
            Assert.IsNotNull(itemDescription1);
            Assert.AreEqual(itemDescription.ToString(), "Item Description ID:0Items:  \nDataSet First code:CODE_ANALOG Second code: CODE_DIGITAL DataSet first value:1 Second value: 2 Capacity:full");
            
        }
    }
}
