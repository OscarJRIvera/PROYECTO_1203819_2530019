using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTO_1203819_2530019.Models
{
    public class LlaveArboPrioridad
    {
        public string CodigoHash { get; set; }
        public double Prioridad { get; set; }
        public static int Compare_Llave_Arbol(LlaveArboPrioridad x, LlaveArboPrioridad y)
        {
            int r = x.Prioridad.CompareTo(y.Prioridad);
            return r;
        }
    }
}
