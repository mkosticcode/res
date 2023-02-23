using LBWorkerLibrary;
using ProjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker
{
    public class CollectionDescription
    {
        int id;
        List<Item> historicalCollection;
        DataSet descriptionDataSet;

        public int Id { get => id; set => id = value; }
        public List<Item> HistoricalCollection { get => historicalCollection; set => historicalCollection = value; }
        public DataSet DescriptionDataSet { get => descriptionDataSet; set => descriptionDataSet = value; }

        public CollectionDescription(int id)
        {
            Id = id;
            HistoricalCollection = new List<Item>();
            DescriptionDataSet = new DataSet();
        }
        public CollectionDescription(ItemDescription itd)
        {
            Id = itd.Id;
            HistoricalCollection = itd.ItemsList;
            DescriptionDataSet = itd.DescriptionDataSet;
        }
        public CollectionDescription()
        {
        }


        public override string ToString()
        {
            string it = "";
            foreach (Item i in HistoricalCollection)
            {
                it += i.ToString();
            }
            return "Item Description ID:" + Id + "Items: " + it + " \nDataSet First code:" + DescriptionDataSet.First
                + " Second code: " + DescriptionDataSet.Second + " DataSet first value:" + DescriptionDataSet.FirstValue
                + " Second value: " + DescriptionDataSet.SecondValue + " Capacity:" + DescriptionDataSet.Capacity;
        }
    }
}
