using ProjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektniZadatak
{
    
    public class ItemDesciption
    {
        int id;
        List<Item> itemsList;
        DataSet descriptionDataSet;

        public int Id { get => id; set => id = value; }

        public ItemDesciption(int id)
        {
            Id = id;
            itemsList = new List<Item>();
            descriptionDataSet = new DataSet();
        }
    }
}
