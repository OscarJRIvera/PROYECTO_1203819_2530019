using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTO_1203819_2530019.Models
{
    public class LlaveArbolPrioridad
    {
        public Int64 CodigoHash { get; set; }
        public double Prioridad { get; set; }
        public static int Compare_Llave_Arbol(LlaveArbolPrioridad x, LlaveArbolPrioridad y)
        {
            int r = x.Prioridad.CompareTo(y.Prioridad);
            return r;
        }
    }
}
