using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTO_1203819_2530019.Models
{
    public class LlaveArbolApellido
    {
        public string Apellido { get; set; }
        public Int64 CodigoHash { get; set; }
        public static int Compare_Llave_Arbol(LlaveArbolApellido x, LlaveArbolApellido y) 
        {
            int r = x.CodigoHash.CompareTo(y.CodigoHash);
            return r;
        }
        public static int Compare_Llave_Arbol2(LlaveArbolApellido x, LlaveArbolApellido y)
        {
            int r = x.Apellido.CompareTo(y.Apellido);
            return r;
        }
    }
}

