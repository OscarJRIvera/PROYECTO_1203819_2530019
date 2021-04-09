using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTO_1203819_2530019.Models
{
    public class LlaveArbolNombre
    {
        public string Nombre { get; set; }
        public static int Compare_Llave_Arbol(LlaveArbolNombre x, LlaveArbolNombre y) 
        {
            int r = x.Nombre.CompareTo(y.Nombre);
            return r;
        }
    }
}
