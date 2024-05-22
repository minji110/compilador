using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace IDE.Objeto
{
    public class Objecto
    {
        private readonly Dictionary<string, string> data;
        private readonly List<string> assembly;
        private readonly List<StringBuilder> binaryCode;
        private int index;

        public Objecto(List<string> assembly)
        {
            this.data = new Dictionary<string, string>();
            this.binaryCode = new List<StringBuilder>();
            this.assembly = assembly;
        }

        public void Run()
        {
            index = 3;
            TagData();
            foreach (var kvp in data)
            {
                Console.WriteLine($"Clave: {kvp.Key}, Valor: {kvp.Value}");
            }

            TagCode();
        }

        private void TagData()
        {
            string[] values;
            List<string> instructions;
            int offset = 0;
            while (assembly.Count > index)
            {
               String x = assembly[index]
                    .ToString()
                    .Trim();
                values = Regex.Split(x,@"\s+");
                    
                instructions = new List<string>(values);
                index++;
                if (instructions.Contains(".code"))
                    break;

                //data[instructions[0]] = Convert.ToString(offset, 2);
                data.Add(instructions[0], Convert.ToString(offset, 2));
                //Console.WriteLine(instructions[0]);
                if (instructions[1].Equals("db"))
                    offset++;
                else
                    offset += 2;
            }
        }

        private void TagCode()
        {
            string[] values;
            List<string> instructions;
            while (assembly.Count > index)
            {
                values = assembly[index]
                    .ToString()
                    .Trim()
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                instructions = new List<string>(values);
                for (int i = 0; i < instructions.Count; i++)
                {
                    instructions[i] = instructions[i].Replace(",", "");
                }

                //Console.WriteLine(instructions[0]);
                index++;
                if (instructions.Count == 0)
                {
                    continue;
                }
                if (instructions.Contains(".exit"))
                    break;

                Console.WriteLine(instructions[0]);
                switch (instructions[0])
                {
                    case "MOV":
                        InstructionMOV(instructions);
                        break;
                    case "ADD":
                        InstructionADD(instructions);
                        break;
                    case "SUB":
                        InstructionSUB(instructions);
                        break;
                    case "AND":
                        InstructionAND(instructions);
                        break;
                    case "ROL":
                        InstructionROL(instructions);
                        break;
                    case "CMP":
                        InstructionCMP(instructions);
                        break;
                    case "JMP":
                        binaryCode.Add(new StringBuilder("11101011").Append(FormatBinary("0", 8)));
                        break;
                    case "JNZ":
                        binaryCode.Add(new StringBuilder(FormatBinary("1110101", 8)).Append(FormatBinary("0", 8)));
                        break;
                    default:
                        binaryCode.Add(new StringBuilder());
                        break;
                }
            }
        }

        private void InstructionCMP(List<string> instructions)
        {
            StringBuilder builder = new StringBuilder();
            string arg1 = instructions[1], arg2 = instructions[2];

            if (arg1.Equals("AX") && arg2.Equals("BX"))
            {
                builder.Append(FormatBinary("111011", 8)).Append("11000011");
            }
            else if (arg1.Equals("AX") && IsNumber(arg2))
            {
                builder.Append(FormatBinary("111101", 8)).Append(FormatBinary(ConvertToBinary(arg2), 8));
            }
            else if (arg1.Equals("AX") && !IsNumber(arg2))
            {
                builder.Append(FormatBinary("111011", 8)).Append(FormatBinary("110", 8)).Append(FormatBinary(ConvertToBinary(data[arg2]), 8));
            }
            else if (!IsNumber(arg1))
            {
                builder.Append("10000011").Append(FormatBinary("111110", 8)).Append(FormatBinary(ConvertToBinary(data[arg1]), 8)).Append(FormatBinary(ConvertToBinary(arg2), 8));
            }
            binaryCode.Add(builder);
        }

        // Métodos de instrucciones 

        private void InstructionAND(List<string> instructions)
        {
            StringBuilder builder = new StringBuilder();
            string arg1 = instructions[1], arg2 = instructions[2];

            if (arg1.Equals("AL") && IsNumber(arg2))
            {
                builder.Append(FormatBinary("100100", 8)).Append(FormatBinary(ConvertToBinary(arg2), 8));
            }
            else if (arg1.Equals("AL") && !IsNumber(arg2))
            {
                builder.Append(FormatBinary("100010", 8)).Append(FormatBinary("110", 8)).Append(FormatBinary(ConvertToBinary(data[arg2]), 8));
            }
            else if (!IsNumber(arg1))
            {
                builder.Append("10000000").Append(FormatBinary("100110", 8)).Append(FormatBinary(ConvertToBinary(data[arg1]), 8)).Append(FormatBinary(ConvertToBinary(arg2), 8));
            }
            binaryCode.Add(builder);
        }

        private void InstructionROL(List<string> instructions)
        {
            StringBuilder builder = new StringBuilder();
            string arg1 = instructions[1];
            string datos;
            data.TryGetValue(arg1, out datos);

            builder.Append("11010000").Append(FormatBinary("110", 8)).Append(FormatBinary(ConvertToBinary(datos), 8));

            binaryCode.Add(builder);
        }

        private void InstructionSUB(List<string> instructions)
        {
            StringBuilder builder = new StringBuilder();
            string arg2 = instructions[2];
            string datos;
            data.TryGetValue(arg2, out datos);

            if (IsNumber(arg2))
            {
                builder.Append(FormatBinary("101101", 8)).Append(FormatBinary(ConvertToBinary(arg2), 16));
            }
            else
            {
                builder.Append(FormatBinary("101011", 8)).Append(FormatBinary("110", 8)).Append(FormatBinary(ConvertToBinary(datos), 16));
            }

            binaryCode.Add(builder);
        }

        private void InstructionADD(List<string> instructions)
        {
            StringBuilder builder = new StringBuilder();
            string arg1 = instructions[1], arg2 = instructions[2];
            string datos;
            data.TryGetValue(arg2, out datos);

            if (arg1.Equals("AX") && IsNumber(arg2))
            {
                builder.Append(FormatBinary("101", 8)).Append(FormatBinary(ConvertToBinary(arg2), 16));
            }
            else if (arg1.Equals("AX") && !IsNumber(arg2))
            {
                builder.Append(FormatBinary("11", 8)).Append(FormatBinary("110", 8)).Append(FormatBinary(ConvertToBinary(datos), 8));
            }
            else if (arg1.Equals("BX"))
            {
                builder.Append(FormatBinary("10000011", 8)).Append(FormatBinary("11000011", 8)).Append(FormatBinary(ConvertToBinary(arg2), 16));
            }

            binaryCode.Add(builder);
        }

        private void InstructionMOV(List<string> instructions)
        {
            StringBuilder builder = new StringBuilder();
            string arg1 = instructions[1], arg2 = instructions[2];
            string datos;
            data.TryGetValue(arg1, out datos);
            string datos2;
            data.TryGetValue(arg2, out datos2);

            Console.WriteLine(IsNumber(arg2));

            if (arg1.Equals("AX") && IsNumber(arg2))
            {
                builder.Append("10111000").Append(FormatBinary(ConvertToBinary(arg2), 16));
            }
            else if (arg1.Equals("AX") && !IsNumber(arg2))
            {
                Console.WriteLine(datos2);
                builder.Append("10100001").Append(FormatBinary(datos2, 16));
            }
            else if (!IsNumber(arg1) && arg2.Equals("AX"))
            {
                Console.WriteLine(arg1);
                builder.Append("10100011").Append(FormatBinary(datos, 16));
            }
            else if (arg1.Equals("AL") && IsNumber(arg2))
            {
                builder.Append("10110000").Append(FormatBinary(ConvertToBinary(arg2), 8));
            }
            else if (arg1.Equals("AL") && !IsNumber(arg2))
            {
                builder.Append("10100000").Append(FormatBinary(datos2, 8));
            }
            else if (!IsNumber(arg1) && arg2.Equals("AL"))
            {
                builder.Append("10100010").Append(FormatBinary(datos, 8));
            }
            else if (arg1.Equals("BX") && IsNumber(arg2))
            {
                builder.Append("10111011").Append(FormatBinary(ConvertToBinary(arg2), 16));
            }
            else if (arg1.Equals("BX") && !IsNumber(arg2))
            {
                builder.Append("10001011").Append(FormatBinary("11110", 8)).Append(FormatBinary(datos2, 16));
            }
            else if (!IsNumber(arg1) && arg2.Equals("AH"))
            {
                builder.Append("10001000").Append(FormatBinary("100110", 8)).Append(FormatBinary(datos, 8));
            }

            binaryCode.Add(builder);
        }

        //da el formato binario
        private string FormatBinary(string value, int size)
        {
            // Rellenar la cadena con ceros a la izquierda hasta el tamaño especificado
            string resultado = value.PadLeft(size, '0');

            // Si el tamaño es menor a 9, devolver el resultado directamente
            if (size < 9)
                return resultado;

            // Dividir la cadena en dos partes
            string alta = resultado.Substring(0, 8);
            string baja = resultado.Substring(8, Math.Min(8, resultado.Length - 8));

            // Retornar las dos partes concatenadas sin coma
            return baja+alta;
        }


        //convertie a binario
        private string ConvertToBinary(string value)
        {
            return Convert.ToString(int.Parse(value), 2);
        }

        private bool IsNumber(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            // Verificar si el valor es un número entero completo
            return int.TryParse(value, out _);
        }


        public Dictionary<string, string> GetData()
        {
            return data;
        }

        public List<StringBuilder> GetBinaryCode()
        {
            return binaryCode;
        }
    }
}
