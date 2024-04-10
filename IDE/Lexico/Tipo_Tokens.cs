using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Lexico
{
    public enum Tipo_Tokens
    {
        OPERADOR_SUM,   
        OPERADOR_RES,   
        OPERADOR_IGU2,  
        OPERADOR_IGU,
        NUMERO,
        IDENTIFICADOR,
        PUNTOYCOMA,
        TIPO_INT,
        TIPO_BOOLEAN,
        LLAVE_IZQ,
        LLAVE_DER,
        PARENTESIS_DER,
        PARENTESIS_IZQ,
        IF_PR,
        WHILE_PR,
        PRINTLN_PR,
        INICIO_PR,
        FIN_PR,
        DESCONOCIDO
    }
}
