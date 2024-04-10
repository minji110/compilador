using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Lexico
{

    class Lexico
    {
           //si cambias el nombre, recuerda ir al tipo_tokens
        public static string DIGITO = "\\d";    //acepta digitos
        public static string LETRAS = "[a-zA-Z]";
        public static string OPERADOR_SUM= "\\+";
        public static string OPERADOR_RES = "-";
        public static string OPERADOR_IGU2 = "==";
        public static string OPERADOR_IGU = "=";
        public static string NUMERO = DIGITO+"+";
        public static string IDENTIFICADOR = LETRAS + "+";
        public static string PUNTOYCOMA = ";";
        public static string TIPO_INT = "int";
        public static string TIPO_BOOLEAN = "boolean";
        public static string LLAVE_IZQ = "{";
        public static string LLAVE_DER = "}";
        public static string PARENTESIS_IZQ = "(";
        public static string PARENTESIS_DER = ")";
        public static string IF_PR = "\\bif\\b";
        public static string WHILE_PR = "\\bwhile\\b";
        public static string PRINTLN_PR = "\\bprintln\\b";
        public static string INICIO_PR = "INICIO";
        public static string FIN_PR= "FIN";
        public static string[] TODO={DIGITO,LETRAS, OPERADOR_SUM,
        OPERADOR_RES, OPERADOR_IGU2, OPERADOR_IGU, NUMERO,
        IDENTIFICADOR, PUNTOYCOMA, TIPO_INT, TIPO_BOOLEAN,
        LLAVE_IZQ, LLAVE_DER, IF_PR, WHILE_PR, PRINTLN_PR };
        public static string DESCONOCIDO= "(?!\\b(" + string.Join("|", TODO) + ")\\b)[\\S_]+";

        //DEFINE POR GRUPO 
       /* public static string PARNER = @"(?<LLAVE_IZQ>" + LLAVE_IZQ + ")" + @"|(?<LLAVE_DER>" + LLAVE_DER + ")"
            + @"|(?<OPERADOR_SUM>" + OPERADOR_SUM + ")" + @"|(?<OPERADOR_RES>" + OPERADOR_RES + ")"
            + @"|(?<OPERADOR_IGU>" + OPERADOR_IGU2 + ")" + @"|(?<OPERADOR_IGUL>" + OPERADOR_IGU + ")"
            + @"|(?<TIPO_INT>" + TIPO_INT + ")" + @"|(?<TIPO_BOOLEAN>" + TIPO_BOOLEAN + ")"
            + @"|(?<IF>" + IF + ")" + @"|(?<WHILE>" + WHILE + ")" + @"|(?<PRINTLN>" + PRINTLN + ")"
            + @"|(?<TODO>" + TODO + ")" + @"|(?<DESCONOCIDO>" + DESCONOCIDO + ")" + @"|(?<DIGITO>" + DIGITO + ")"
            + @"|(?<LETRAS>" + LETRAS + ")"+ @"|(?<PUNTOYCOMA>" + PUNTOYCOMA + ")"
            + @"|(?<PARENTESIS_DER>" + PARENTESIS_DER + ")"+ @"|(?<PARENTESIS_IZQ>" + PARENTESIS_IZQ + ")"
            + @"|(?<INICIO_PR>" + INICIO_PR + ")" + @"|(?<FIN>" + FIN_PR+ ")"; */

    }
}
