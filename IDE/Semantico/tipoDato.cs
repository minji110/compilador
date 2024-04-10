using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDE.Lexico;

namespace IDE.Semantico
{
    class tipoDato
    {
        public Tipo_Tokens Tipo_Dato;
        public String Nombre { get; }
        public String Valor{get; set;}

        public tipoDato(Tipo_Tokens tipo_dato, String nombre)
        {
            this.Tipo_Dato = tipo_dato;
            this.Nombre = nombre;
        }

        public String toString()
        {
            return "Tipo de dato: " + "tipo: "+this.Tipo_Dato+", nombre: "+this.Nombre;
        }

    }
}
