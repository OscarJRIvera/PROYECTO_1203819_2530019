using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTO_1203819_2530019.Models
{
    public class LlaveArbolNumeroDR
    {
        public string NumeroDR { get; set; }
        public static int Compare_Llave_Arbol(LlaveArbolNumeroDR x, LlaveArbolNumeroDR y) 
        {
            int r = x.NumeroDR.CompareTo(y.NumeroDR);
            return r;
        }
    }
}
