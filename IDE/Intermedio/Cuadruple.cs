using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Intermedio
{
    public class Cuadruple
    {
        public string Operator { get; }
        public string Arg1 { get; }
        public string Arg2 { get; }
        public string Result { get; set; }

        public Cuadruple(string op, string arg1, string arg2, string result)
        {
            Operator = op;
            Arg1 = arg1;
            Arg2 = arg2;
            Result = result;
        }

        public override string ToString()
        {
            return "Quadruple{" +
                   "Operator='" + Operator + '\'' +
                   ", Arg1='" + Arg1 + '\'' +
                   ", Arg2='" + Arg2 + '\'' +
                   ", Result='" + Result + '\'' +
                   '}';
        }
    }

}
