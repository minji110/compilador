using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Lexico
{
    public class Tokens
    {
        public string TOKENS { get; set; }
        public Tipo_Tokens LEXEMAS { get; set; }

        public Tokens(string token, Tipo_Tokens lexema)
        {
            TOKENS = token;
            LEXEMAS = lexema;
        }

        public override string ToString()
        {
            return "Token: " + TOKENS + " Lexema: " + LEXEMAS;
        }
    }
}
