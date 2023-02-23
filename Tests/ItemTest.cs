using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ItemTest
    {
        [TestMethod]
        public void TestItem()
        {

            Item item = new Item();
            Item item2 = new Item(ProjectLibrary.Codes.CODE_ANALOG,5);

            Assert.IsNotNull(item);
            Assert.IsNotNull(item2);
            Assert.AreEqual(item.ToString(), "Code:ERROR|| Value:0");
            Assert.AreEqual(item2.ToString(), "Code:CODE_ANALOG|| Value:5");
            
        }
    }
}
