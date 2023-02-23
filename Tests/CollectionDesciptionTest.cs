using LBWorkerLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectLibrary;
using ProjektniZadatak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker;

namespace Tests
{
    [TestClass]
    public class CollectionDesciptionTest
    {
        [TestMethod]
        public void TestCollectionDescription()
        {
            
            ItemDescription itemDescription = new ItemDescription();
            CollectionDescription cD = new CollectionDescription(1);
            CollectionDescription collectionDescription = new CollectionDescription(itemDescription);
            collectionDescription.HistoricalCollection = new List<ProjectLibrary.Item>();
            collectionDescription.DescriptionDataSet = new DataSet(1, Codes.CODE_ANALOG, Codes.CODE_DIGITAL, 1, 2, EnteredData.full);
            DataSetList dataSetList = new DataSetList();

            Assert.IsNotNull(cD);
            Assert.IsNotNull(collectionDescription);
            Assert.AreEqual(collectionDescription.ToString(), "Item Description ID:0Items:  \nDataSet First code:CODE_ANALOG Second code: CODE_DIGITAL DataSet first value:1 Second value: 2 Capacity:full");

        }
    }
}
