using System;
using System.Collections.Generic;
using IDE.Lexico;
using IDE.Intermedio;

namespace IDE.Intermedio
{
    public class Direction
    {
        public List<Cuadruple> quadruples { get; }
        public HashSet<string> temporal;
        private List<Tokens> tokens;
        private Stack<Stack<int>> stacksIf;
        private Stack<Stack<int>> stacksWhile;
        private int index;

        public Direction(List<Tokens> tokens)
        {
            quadruples = new List<Cuadruple>();
            temporal = new HashSet<string>();
            stacksIf = new Stack<Stack<int>>();
            stacksWhile = new Stack<Stack<int>>();
            index = 0;
            this.tokens = tokens;
        }

        public void Run()
        {
            while (tokens.Count > index)
                Sentences();
        }

        public void Sentences()
        {
            Stack<int> stackWhile;
            Console.WriteLine(index);
            switch (tokens[index].LEXEMAS)
            {
                case Tipo_Tokens.LLAVE_IZQ:
                case Tipo_Tokens.INICIO_PR:
                    stacksIf.Push(new Stack<int>());
                    stacksWhile.Push(new Stack<int>());
                    index++;
                    break;
                case Tipo_Tokens.WHILE_PR:
                    stackWhile = stacksWhile.Peek();
                    stackWhile.Push(quadruples.Count);
                    ResolveIfWhile(Tipo_Tokens.LLAVE_IZQ);
                    break;
                case Tipo_Tokens.IF_PR:
                    ResolveIfWhile(Tipo_Tokens.LLAVE_IZQ);
                    break;
                case Tipo_Tokens.IDENTIFICADOR:
                    ResolveIdentifier(Tipo_Tokens.PUNTOYCOMA);
                    break;
                case Tipo_Tokens.LLAVE_DER:
                case Tipo_Tokens.FIN_PR:
                    ResolveBrace();
                    index++;
                    break;
                default:
                    index++;
                    break;
            }
        }

        private void ResolveBrace()
        {
            Stack<int> stackIf = stacksIf.Pop();
            while (stackIf.Count > 0)
            {
                int position = stackIf.Pop();
                Cuadruple quadruple = quadruples[position];
                int result = quadruple.Operator == "while" ? quadruples.Count + 1 : quadruples.Count;
                quadruple.Result = result.ToString();
            }

            Stack<int> stackWhile = stacksWhile.Pop();
            while (stackWhile.Count > 0)
            {
                int position = stackWhile.Pop();
                quadruples.Add(new Cuadruple("jump", null, null, position.ToString()));
            }

            if (stacksIf.Count <= 0)
            {
                return;
            }
            stackIf = stacksIf.Pop();

            while (stackIf.Count > 0)
            {
                int position = stackIf.Pop();
                Cuadruple quadruple = quadruples[position];
                int result = quadruple.Operator == "while" ? quadruples.Count + 1 : quadruples.Count;
                quadruple.Result = result.ToString();
            }

            stacksIf.Push(stackIf);

            
        }


        private void ResolveIfWhile(params Tipo_Tokens[] type)
        {
            int x = 0, count = 1;
            List<string> postfix = GetPostfix(type);
            postfix.ForEach(Console.WriteLine);
            Console.WriteLine(postfix.Count);
            while (postfix.Count != 1)
            {
                string op = postfix[x];
                string result = "_B" + count;
                if (!Posfijo.ops.ContainsKey(op))
                {
                    x++;
                    continue;
                }

                switch (op)
                {
                    case "while":
                    case "if":

                        Stack<int> stack = stacksIf.Pop();
                        stack.Push(quadruples.Count);
                        x -= 1;
                        quadruples.Add(new Cuadruple(
                            op,
                            postfix[x],
                            null,
                            null
                        ));
                        stacksIf.Push(stack);
                        postfix.RemoveAt(x);
                        break;
                    default:
                        x -= 2;
                        quadruples.Add(new Cuadruple(
                            op,
                            postfix[x],
                            postfix[x + 1],
                            result
                        ));
                        temporal.Add(result);
                        postfix.RemoveAt(x);
                        postfix.RemoveAt(x);
                        break;
                }
                postfix[x] = result;

                count++;
            }
        }

        private void ResolveIdentifier(params Tipo_Tokens[] type)
        {
            int x = 0, count = 1;
            List<string> postfix = GetPostfix(type);
            while (postfix.Count != 1)
            {
                string op = postfix[x];
                string result = "_T" + count;
                temporal.Add(result);
                if (!Posfijo.ops.ContainsKey(op))
                {
                    x++;
                    continue;
                }

                x -= 2;

                string value1 = postfix[x];
                string value2 = postfix[x + 1];
                postfix.RemoveAt(x);
                postfix.RemoveAt(x);
                if (op.Equals("="))
                {
                    result = value1;
                    value1 = value2;
                    value2 = null;
                }

                quadruples.Add(new Cuadruple(
                    op,
                    value1,
                    value2,
                    result
                ));
                temporal.Add(result);
                postfix[x] = result;
                count++;
            }
        }

        private List<string> GetPostfix(params Tipo_Tokens[] type)
        {
            int index_aux = index;
            List<Tokens> list = new List<Tokens>(tokens.GetRange(index_aux, GetIndex(type)-index_aux));
            return Posfijo.Run(list);
        }

        private int GetIndex(params Tipo_Tokens[] token)
        {
            while (!IsToken(token))
                index++;
            return index;
        }

        private bool IsToken(params Tipo_Tokens[] list)
        {
            Tipo_Tokens token = tokens[index].LEXEMAS;
            List<Tipo_Tokens> types = new List<Tipo_Tokens>(list);
            return types.Contains(token);

        }
    }
}
