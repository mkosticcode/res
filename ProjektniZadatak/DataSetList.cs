using LBWorkerLibrary;
using ProjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektniZadatak
{
    // date kombinacije dataSet
    public class DataSetList
    {
        List<DataSet> codeDatas;

        public DataSetList()
        {
            codeDatas = new List<DataSet>();

            DataSet first = new DataSet(1, Codes.CODE_ANALOG, Codes.CODE_DIGITAL,0,0, EnteredData.empty);
            DataSet second = new DataSet(2, Codes.CODE_CUSTOM, Codes.CODE_LIMITSET, 0, 0, EnteredData.empty);
            DataSet third = new DataSet(3, Codes.CODE_SINGLENOE, Codes.CODE_MULTIPLENODE, 0, 0, EnteredData.empty);
            DataSet fourth = new DataSet(4, Codes.CODE_CONSUMER, Codes.CODE_SOURCE, 0, 0, EnteredData.empty);
            codeDatas.Add(first);
            codeDatas.Add(second);
            codeDatas.Add(third);
            codeDatas.Add(fourth);



        }
    }
}
