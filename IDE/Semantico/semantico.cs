using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDE.Lexico;
using NCalc;

namespace IDE.Semantico
{
    class semantico
    {
        public List<Tokens> token;
        public Dictionary<String, tipoDato > stack;
        public int index;
        public semantico()
        {
            stack = new Dictionary <String, tipoDato> ();
        }


        //dice semantico1 porque ya se usa semantico
        public void semantico1(List<Tokens> tokens)
        {

            while (tokens.Count - 1 > index)
            {
                definirTokens(tokens);
            }

        }

        public void definirTokens(List<Tokens> tokens)
        {
            Tipo_Tokens tipo;
            String nombre;
            //variable para guardar en el diccionario
            tipoDato variable;

            switch (tokens[index].LEXEMAS)
            {
                case Tipo_Tokens.TIPO_BOOLEAN:
                    tipo = devolverToken(tokens, Tipo_Tokens.TIPO_BOOLEAN).LEXEMAS;
                    nombre = devolverToken(tokens, Tipo_Tokens.IDENTIFICADOR).TOKENS;
                    stack.TryGetValue(nombre, out variable);
                    if (variable!= null)
                    {
                        throw new Exception("la variable " + nombre + " ya fue declarada.");
                    }
                    variable = new tipoDato(tipo, nombre);
                    stack.Add(nombre, variable);
                    break;
                case Tipo_Tokens.TIPO_INT:
                    tipo = devolverToken(tokens, Tipo_Tokens.TIPO_INT).LEXEMAS;
                    nombre = devolverToken(tokens, Tipo_Tokens.IDENTIFICADOR).TOKENS;
                    stack.TryGetValue(nombre, out variable);
                    if (variable != null)
                    {
                        throw new Exception("la variable " + nombre + " ya fue declarada.");
                    }
                    variable = new tipoDato(tipo, nombre);
                    stack.Add(nombre, variable);
                    break;
                case Tipo_Tokens.IDENTIFICADOR:
                    nombre = devolverToken(tokens, Tipo_Tokens.IDENTIFICADOR).TOKENS;
                    stack.TryGetValue(nombre, out variable);
                    if (variable == null)
                    {
                        throw new Exception("La variable " + nombre + " no existe");
                    }
                    devolverToken(tokens, Tipo_Tokens.OPERADOR_IGU);
                    
                    if (variable.Tipo_Dato==Tipo_Tokens.TIPO_INT)
                    {
                        variable.Valor = tipoInt(tokens);
                    }
                    else
                    {
                        variable.Valor = esBoleano(tokens);
                    }
                    break;
                case Tipo_Tokens.IF_PR:
                case Tipo_Tokens.WHILE_PR:
                    devolverToken(tokens, Tipo_Tokens.IF_PR, Tipo_Tokens.WHILE_PR);
                    devolverToken(tokens, Tipo_Tokens.PARENTESIS_IZQ);
                    esBoleano(tokens);
                    break;
                /*case Tipo_Tokens.PRINTLN_PR:
                    index += 3;
                    break;*/
                default:
                    index++;
                    break;
            }

        }

        //busca las expresiones
        public String tipoInt(List<Tokens> tokens)
        {
            String expresion = "";
            do
            {
                expresion += obtenerEntero(tokens);
                //Console.WriteLine("aA");
                if (!esToken(tokens, Tipo_Tokens.OPERADOR_SUM, Tipo_Tokens.OPERADOR_RES))
                {
                    // Console.WriteLine("aA");
                    break;
                }
                
                expresion += devolverToken(tokens, Tipo_Tokens.OPERADOR_SUM, Tipo_Tokens.OPERADOR_RES).TOKENS;
            }
            while (true);
            Expression evaluator = new Expression(expresion);
            //Console.WriteLine("aA");
            //Console.WriteLine(evaluator.Evaluate().ToString());
            //Console.WriteLine("aA");
            return evaluator.Evaluate().ToString();
        } 

        public Boolean esToken(List<Tokens> tokens, params Tipo_Tokens[] a)
        {
            Tipo_Tokens tokenss = tokens[index].LEXEMAS;
            return a.Contains(tokenss);
        }
        

        public String obtenerEntero(List<Tokens> tokens)
        {
            Tokens nombre = devolverToken(tokens,Tipo_Tokens.IDENTIFICADOR,Tipo_Tokens.NUMERO);
            if (nombre.LEXEMAS==Tipo_Tokens.IDENTIFICADOR)
            {
                return obtenerTipo(nombre).Valor;
            }
            return nombre.TOKENS;
        }

        public tipoDato obtenerTipo(Tokens token)
        {
            //guardar el token de la pila
            tipoDato nombre;
            stack.TryGetValue(token.TOKENS,out nombre);
            if(nombre== null)
            {
                throw new Exception("Error semantico. la variable "+nombre.Nombre+" no esta declarada.");
            }
            if (nombre.Tipo_Dato!=Tipo_Tokens.TIPO_INT)
            {
                throw new Exception("la variable "+nombre.Nombre+" no esta definida como entero");
            }
            return nombre;
        }


        public Tokens devolverToken(List<Tokens> tokens, params Tipo_Tokens[] a)
        {
            
            if (tokens.Count <= index)
            {
                throw new Exception("Error semantico");
            }
            Tokens tokenss = tokens[index];
            //a es la lista de comparaciones 
            if (a.Contains(tokenss.LEXEMAS))
            {
                index++;
                return tokenss;
            }
            else
            {
                throw new Exception("Error Semantico");
            }
        }
        //
        public String esBoleano(List<Tokens> tokens)
        {
            String expresion = "";
            tipoDato dato;
            switch (tokens[index].LEXEMAS)
            {
                case Tipo_Tokens.IDENTIFICADOR:
                    stack.TryGetValue(tokens[index].TOKENS,out dato);
                    if (dato == null)
                    {
                        throw new Exception("Variable no definida");
                    }
                    if (dato.Tipo_Dato == Tipo_Tokens.TIPO_INT)
                    {
                        expresion += tipoInt(tokens);
                        expresion += devolverToken(tokens, Tipo_Tokens.OPERADOR_IGU2).TOKENS;
                        expresion += tipoInt(tokens);
                    }else{
                        expresion += dato.Valor;
                    }
                    break;
                case Tipo_Tokens.NUMERO:
                    expresion+=tipoInt(tokens);
                    expresion += devolverToken(tokens, Tipo_Tokens.OPERADOR_IGU2).TOKENS;
                    expresion += tipoInt(tokens);
                    break;
            }
            Expression evaluator = new Expression(expresion);
            Console.WriteLine(expresion);
            return evaluator.Evaluate().ToString(); 
        }
    }
}
