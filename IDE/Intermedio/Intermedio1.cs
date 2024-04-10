using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDE.Lexico;

namespace IDE.Intermedio
{
    public class Intermedio1
    {
        private List<Cuadruple> quadruples;
        private readonly Dictionary<string, Cuadruple> variable;
        private Dictionary<int, StringBuilder> tags;
        private readonly List<StringBuilder> assembly, code;

        public Intermedio1(List<Tokens> tokenss)
        {
            assembly = new List<StringBuilder>();
            code = new List<StringBuilder>();
            variable = new Dictionary<string, Cuadruple>();
            tags = new Dictionary<int, StringBuilder>();
            StartDirection(tokenss);
        }

        public void Run()
        {
            assembly.Add(new StringBuilder(".model small"));
            assembly.Add(new StringBuilder(".stack"));
            assembly.Add(new StringBuilder(".data"));
            StartData();
            assembly.Add(new StringBuilder(".code"));
            //quadruples.ForEach(Console.WriteLine);
            foreach (var quadruple in quadruples)
                StartCode(quadruple);
            AddTags();
            assembly.AddRange(code);
            assembly.Add(new StringBuilder(".exit"));
        }

        private void StartData()
        {
            assembly.Add(new StringBuilder("\t").Append("true").Append("\t").Append("db").Append("\t").Append("1"));
            assembly.Add(new StringBuilder("\t").Append("false").Append("\t").Append("db").Append("\t").Append("0"));

            foreach (var quadruple in quadruples)
            {
                if (int.TryParse(quadruple.Result, out _))
                    continue;
                else if (variable.ContainsKey(quadruple.Result))
                    continue;

                variable.Add(quadruple.Result, quadruple);
            }

            foreach (KeyValuePair <string, Cuadruple> value in variable)
            {
                var result = new StringBuilder();
                switch (value.Value.Operator)
                {
                    case "==":
                        result.Append("\t")
                            .Append(value.Value.Result)
                            .Append("\t")
                            .Append("db")
                            .Append("\t")
                            .Append("?");
                        break;
                    case "+":
                    case "-":
                        result.Append("\t")
                            .Append(value.Value.Result)
                            .Append("\t")
                            .Append("dw")
                            .Append("\t")
                            .Append("?");
                        break;
                    case "=":
                        var type = int.TryParse(value.Value.Arg1, out _) || value.Value.Arg1.StartsWith("_T") ? "dw" : "db";
                        result.Append("\t")
                            .Append(value.Value.Result)
                            .Append("\t")
                            .Append(type)
                            .Append("\t")
                            .Append("?");
                        break;
                }

                assembly.Add(result);
            }
        }

        private void StartCode(Cuadruple quadruple)
        {
            var result = new StringBuilder();
            int size;
            switch (quadruple.Operator)
            {
                case "=":
                    result.Append("\t").Append("MOV AX, ").Append(quadruple.Arg1).Append("\r\n");
                    result.Append("\t").Append("MOV ").Append(quadruple.Result).Append(", AX");
                    break;
                case "+":
                    result.Append("\t").Append("MOV AX, ").Append(quadruple.Arg1).Append("\r\n");
                    result.Append("\t").Append("ADD AX, ").Append(quadruple.Arg2).Append("\r\n");
                    result.Append("\t").Append("MOV ").Append(quadruple.Result).Append(", AX");
                    break;
                case "-":
                    result.Append("\t").Append("MOV AX, ").Append(quadruple.Arg1).Append("\r\n");
                    result.Append("\t").Append("SUB AX, ").Append(quadruple.Arg2).Append("\r\n");
                    result.Append("\t").Append("MOV ").Append(quadruple.Result).Append(", AX");
                    break;
                case "if":
                    size = int.Parse(quadruple.Result);
                    result.Append("\t").Append("CMP ").Append(quadruple.Arg1).Append(", 1").Append("\r\n");
                    result.Append("\t").Append("JNZ ").Append("Else").Append(size);
                    tags[size] = new StringBuilder("\t").Append("Else").Append(size).Append(":");
                    break;
                case "==":
                    result.Append("\t").Append("MOV AX, ").Append(quadruple.Arg1).Append("\r\n");
                    result.Append("\t").Append("CMP AX, ").Append(quadruple.Arg2).Append("\r\n");
                    result.Append("\t").Append("LAHF").Append("\n");
                    result.Append("\t").Append("MOV ").Append(quadruple.Result).Append(", AH").Append("\r\n");
                    result.Append("\t").Append("ROL ").Append(quadruple.Result).Append(", 2").Append("\r\n");
                    result.Append("\t").Append("AND ").Append(quadruple.Result).Append(", 1");
                    break;
                case "while":
                    size = int.Parse(quadruple.Result);
                    result.Append("\t").Append("CMP ").Append(quadruple.Arg1).Append(", 1").Append("\r\n");
                    result.Append("\t").Append("JNZ ").Append("FinishWhile").Append(size);
                    tags[int.Parse(quadruple.Result)] = new StringBuilder("\t").Append("FinishWhile").Append(size).Append(":");
                    break;
                case "jump":
                    size = int.Parse(quadruple.Result);
                    result.Append("\t").Append("JMP StartWhile").Append(quadruple.Result);
                    tags[size] = new StringBuilder("\t").Append("StartWhile").Append(size).Append(":");
                    break;
            }
            result.Append("\r\n");
            code.Add(result);
        }

        private void AddTags()
        {

            tags = SortTags();
            
            foreach (var tag in tags)
            {
                code.Insert(tag.Key,tag.Value);
            }
            
        }

        public Dictionary<int, StringBuilder> SortTags()
        {
            return tags.OrderByDescending(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public void StartDirection(List<Tokens> tokens)
        {
            var direction = new Direction(tokens);
            direction.Run();
            quadruples = direction.quadruples;
        }

        public List<StringBuilder> GetAssembly()
        {
            return assembly;
        }
    }
}

