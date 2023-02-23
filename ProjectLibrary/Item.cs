using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibrary
{
    [Serializable]
    public class Item
    {
        Codes code;
        double value;

        public Codes Code { get => code; set => code = value; }
        public double Value { get => value; set => this.value = value; }

        public Item()
        {
        }

        public Item(Codes code, double value)
        {
            Code = code;
            Value = value;
        }

        public override string ToString()
        {
            double var2 = Math.Round(Value, 2);
            String s = "Code:" + Code + "|| Value:" + var2;
            return s;
        }
    }
}
