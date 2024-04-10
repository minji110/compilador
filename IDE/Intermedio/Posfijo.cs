using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDE.Lexico;

namespace IDE.Intermedio
{
    public class Posfijo
    {
        public static readonly Dictionary<string, Operator> ops = new Dictionary<string, Operator>
        {
            { "=", Operator.EQUAL },
            { "+", Operator.ADD },
            { "-", Operator.SUBTRACT },
            { "==", Operator.EQUAL_EQUAL },        //==
            { "while", Operator.WHILE_RESERVED },
            { "if", Operator.IF_RESERVED },

        };

        private static bool IsHigherPrec(string op, string sub)
        {
            if (!ops.ContainsKey(sub))
            {
                return false;
            }

            int argumento1 = (int)ops[sub];
           
            int argumento2 = (int)ops[op];
            return ops.ContainsKey(sub) && argumento1 >= argumento2;
        }

        public static List<string> Run(List<Tokens> args)
        {
            List<string> output = new List<string>();
            Stack<string> stack = new Stack<string>();
            foreach (Tokens token in args) // Cambiado de Tokens a Tokens
            {
                string tokens = token.TOKENS; // Acceso a la propiedad correcta del token
                
                if (ops.ContainsKey(tokens))
                {
                    while (stack.Count > 0 && IsHigherPrec(tokens, stack.Peek()))
                        output.Add(stack.Pop());
                    stack.Push(tokens);
                }
                else if (tokens.Equals("("))
                    stack.Push(tokens);
                else if (tokens.Equals(")"))
                {
                    while (!stack.Peek().Equals("("))
                        output.Add(stack.Pop());
                    stack.Pop();
                }
                else
                    output.Add(tokens);
            }

            while (stack.Count > 0)
                output.Add(stack.Pop());
            return output;
        }

    }
}
