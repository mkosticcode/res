using ProjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LBWorkerLibrary
{
    public enum EnteredData : int
    {
        error=0,
        empty = 1,
        left = 2,
        right = 3,
        full = 4,

    }
    [Serializable]
    public class DataSet
    {
        int order;
        Codes first;
        Codes second;
        
        EnteredData capacity;
        double firstValue;
        double secondValue;

        public int Order { get => order; set => order = value; }
        public Codes First { get => first; set => first = value; }
        public Codes Second { get => second; set => second = value; }
       // [EnumMemberAttribute]
        [DataMember]
        public EnteredData Capacity { get => capacity; set => capacity = value; }
        public double FirstValue { get => firstValue; set => firstValue = value; }
        public double SecondValue { get => secondValue; set => secondValue = value; }

        public DataSet()
        {
        }

        public DataSet(int order, Codes first, Codes second,double firstValue,double secondValue, EnteredData capacity)
        {
            Order = order;
            First = first;
            Second = second;
            FirstValue = firstValue;
            SecondValue = secondValue;
            Capacity = capacity;
        }
    }
}
