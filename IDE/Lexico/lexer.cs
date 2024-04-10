using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Lexico
{
    class lexer : Lexico
    {
        public List<Tokens> Tokens { get; set; }
        public lexer()
        {
            Tokens = new List<Tokens>();
        }

        public int i;
        public void Cadena_A_Tokens(string texto)
        {
            texto = texto.Trim();
            for (i = 0; i < texto.Length; i++)
            {
                //Console.WriteLine(i+" "+texto[i]);
                if(char.IsWhiteSpace(texto[i]))
                {
                    continue;
                }
                Console.WriteLine(i + " " + texto[i]);
                if (buscar(i, texto, "=="))
                {
                    Tokens.Add(new Tokens("==", Tipo_Tokens.OPERADOR_IGU2));
                    continue;
                }
                if (texto[i]=='+')
                {
                    Tokens.Add(new Tokens("+", Tipo_Tokens.OPERADOR_SUM));
                    continue;
                }
                if (texto[i]=='-')
                {
                    Tokens.Add(new Tokens("-", Tipo_Tokens.OPERADOR_RES));
                    continue;
                }
                if (texto[i]=='=')
                {
                    Tokens.Add(new Tokens("=", Tipo_Tokens.OPERADOR_IGU));
                    continue;
                }
                if (buscar(i, texto, "INICIO"))
                {
                    Tokens.Add(new Tokens("INICIO", Tipo_Tokens.INICIO_PR));
                    continue;
                }
                if (buscar(i, texto, "FIN"))
                {
                    Tokens.Add(new Tokens("FIN", Tipo_Tokens.FIN_PR));
                    continue;
                }
                if (buscar(i, texto, "if"))
                {
                    Tokens.Add(new Tokens("if", Tipo_Tokens.IF_PR));
                    continue;
                }
                if (buscar(i, texto, "while"))
                {
                    Tokens.Add(new Tokens("while", Tipo_Tokens.WHILE_PR));
                    continue;
                }
                if (buscar(i, texto, "println"))
                {
                    Tokens.Add(new Tokens("println", Tipo_Tokens.PRINTLN_PR));
                    continue;
                }
                if (buscar(i, texto, "int"))
                {
                    Tokens.Add(new Tokens("int", Tipo_Tokens.TIPO_INT));
                    continue;
                }
                if (buscar(i, texto, "boolean"))
                {
                    Tokens.Add(new Tokens("boolean", Tipo_Tokens.TIPO_BOOLEAN));
                    continue;
                }
                if (texto[i]==';')
                {
                    Tokens.Add(new Tokens(";", Tipo_Tokens.PUNTOYCOMA));
                    continue;
                }
                if (texto[i]=='{')
                {
                    Tokens.Add(new Tokens("{", Tipo_Tokens.LLAVE_IZQ));
                    continue;
                }
                if (texto[i]=='}')
                {
                    Tokens.Add(new Tokens("}", Tipo_Tokens.LLAVE_DER));
                    continue;
                }
                if (texto[i]=='(')
                {
                    Tokens.Add(new Tokens("(", Tipo_Tokens.PARENTESIS_IZQ));
                    continue;
                }
                if (texto[i]==')')
                {
                    Tokens.Add(new Tokens(")", Tipo_Tokens.PARENTESIS_DER));
                    continue;
                }
                string identificador = buscar_identificadores(i,texto);
                if (identificador != null)
                {
                    Tokens.Add(new Tokens(identificador, Tipo_Tokens.IDENTIFICADOR));
                    continue;
                }
                string numeros = buscar_numeros(i, texto);
                if (numeros != null)
                {
                    Tokens.Add(new Tokens(numeros, Tipo_Tokens.NUMERO));
                    continue;
                }
                Tokens.Add(new Tokens(texto[i] + "", Tipo_Tokens.DESCONOCIDO));
            }

        }

        public bool buscar(int index, string texto, string palabra)
        {
            string aux="";
            for (int i = 0; i<palabra.Length; i++)
            {
                if (palabra[i].Equals(texto[index+i]))
                {
                    aux += texto[index + i];
                }
                else
                {
                    return false;
                }
            }
            if (aux.Equals(palabra))
            {
                i += palabra.Length-1;
                return true;
            }
            return false;
        }

        public string buscar_identificadores(int index, string texto)
        {
            string aux = null;
            for (int i = index; i<texto.Length; i++)
            {
                if (char.IsWhiteSpace(texto[i]))
                {
                    if (aux == null)
                    {
                        return aux;
                    }
                    this.i += aux.Length-1;
                    return aux;
                }
                if (char.IsLetter(texto[i]))
                {
                    if (aux == null)
                    {
                        aux = "";
                    }
                    aux += texto[i];
                }
                else
                {
                    if (aux == null)
                    {
                        return aux;
                    }
                    this.i += aux.Length - 1;
                    return aux;
                }
            }
            this.i += aux.Length - 1;
            return aux;
        }

        public string buscar_numeros(int index, string texto)
        {
            string aux = null;
            for (int i = index; i < texto.Length; i++)
            {
                if (char.IsWhiteSpace(texto[i]))
                {
                    if (aux == null)
                    {
                        return aux;
                    }
                    this.i += aux.Length - 1;
                    return aux;
                }
                if (char.IsDigit(texto[i]))
                {
                    if (aux == null)
                    {
                        aux = "";
                    }
                    aux += texto[i];
                }
                else
                {
                    if (aux == null)
                    {
                        return aux;
                    }
                    this.i += aux.Length - 1;
                    return aux;
                }
            }
            return aux;
        }

    }
}


