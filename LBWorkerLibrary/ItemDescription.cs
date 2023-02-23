using ProjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBWorkerLibrary
{
    [Serializable]
    public class ItemDescription
    {
        int id;
        List<Item> itemsList;
        DataSet descriptionDataSet;

        public int Id { get => id; set => id = value; }
        public List<Item> ItemsList { get => itemsList; set => itemsList = value; }
        public DataSet DescriptionDataSet { get => descriptionDataSet; set => descriptionDataSet = value; }

        public ItemDescription(int id)
        {
            Id = id;
            ItemsList = new List<Item>();
            DescriptionDataSet = new DataSet();
        }

        public ItemDescription()
        {
        }

        public override string ToString()
        {
            string it="";
            foreach(Item i in ItemsList)
            {
                it += i.ToString();
            }
            return "Item Description ID:" + Id + "Items: " + it + " \nDataSet First code:" + DescriptionDataSet.First
                + " Second code: " +DescriptionDataSet.Second + " DataSet first value:" + DescriptionDataSet.FirstValue
                + " Second value: " + DescriptionDataSet.SecondValue + " Capacity:" + DescriptionDataSet.Capacity;                                                           
        }
    }
}
