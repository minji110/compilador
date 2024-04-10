using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDE.Lexico;

namespace IDE.Parser
{
    class Sintactico
    {
        public int index;
        public void parser(List<Tokens> tokens)
        {
            mensajes(tokens, Tipo_Tokens.INICIO_PR);
            while (tokens.Count-1 > index)
            {
                declaracionesLista(tokens);
            }
            mensajes(tokens, Tipo_Tokens.FIN_PR);
        }

        public bool mensajeOpcional(List<Tokens> tokens, params Tipo_Tokens[] a)
        {
            if (tokens.Count <= index)
            {
                throw new Exception("Se acabaron los tokens. Error sintactico");
            }
            Tipo_Tokens token = tokens[index].LEXEMAS;
            //a es la lista de comparaciones 
            if (a.Contains(token))
            {
                index++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void bloque(List<Tokens> tokens)
        {
            
            mensajes(tokens, Tipo_Tokens.LLAVE_IZQ);
            //valida casi todo lo que hay dentro de las llaves
            while (!mensajeOpcional(tokens,Tipo_Tokens.LLAVE_DER))
            {
                declaracionesLista(tokens);
            }
        }
        public void mensajes(List<Tokens> tokens, params Tipo_Tokens [] a)
        {
            if (tokens.Count <= index)
            {
                throw new Exception("Se acabaron los tokens. Error sintactico");
            }
            Tipo_Tokens token = tokens[index].LEXEMAS;
            //a es la lista de comparaciones 
            if (a.Contains(token))
            {
                index++;
            }
            else
            {
                throw new Exception(" Error sintactico");
            }
        }

        public void declaracionesLista(List<Tokens> tokens)
        {
            switch (tokens[index].LEXEMAS)
            {
                case Tipo_Tokens.TIPO_BOOLEAN:
                case Tipo_Tokens.TIPO_INT:
                    mensajes(tokens, Tipo_Tokens.TIPO_BOOLEAN, Tipo_Tokens.TIPO_INT);
                    mensajes(tokens, Tipo_Tokens.IDENTIFICADOR);
                    mensajes(tokens, Tipo_Tokens.PUNTOYCOMA);
                    break;
                case Tipo_Tokens.IDENTIFICADOR:
                    mensajes(tokens, Tipo_Tokens.IDENTIFICADOR);
                    mensajes(tokens, Tipo_Tokens.OPERADOR_IGU);
                    expresionLista(tokens);
                    mensajes(tokens, Tipo_Tokens.PUNTOYCOMA);
                    break;
                /*case Tipo_Tokens.PRINTLN_PR:
                    mensajes(tokens, Tipo_Tokens.PRINTLN_PR);
                    mensajes(tokens, Tipo_Tokens.IDENTIFICADOR);
                    mensajes(tokens, Tipo_Tokens.PUNTOYCOMA);
                    break;*/
                case Tipo_Tokens.IF_PR:
                    mensajes(tokens, Tipo_Tokens.IF_PR);
                    mensajes(tokens, Tipo_Tokens.PARENTESIS_IZQ);
                    expresionLista(tokens);
                    mensajes(tokens, Tipo_Tokens.PARENTESIS_DER);
                    bloque(tokens);
                    break;
                case Tipo_Tokens.WHILE_PR:
                    mensajes(tokens, Tipo_Tokens.WHILE_PR);
                    mensajes(tokens, Tipo_Tokens.PARENTESIS_IZQ);
                    expresionLista(tokens);
                    mensajes(tokens, Tipo_Tokens.PARENTESIS_DER);
                    bloque(tokens);
                    break;
            }
            
        }

        public void expresionLista(List<Tokens> tokens)
        {
            do
            {
                mensajes(tokens,Tipo_Tokens.IDENTIFICADOR,Tipo_Tokens.NUMERO);
            } while (mensajeOpcional(tokens,Tipo_Tokens.OPERADOR_IGU2,Tipo_Tokens.OPERADOR_SUM,Tipo_Tokens.OPERADOR_RES));
        }
    }

}
