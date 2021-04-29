using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTO_1203819_2530019.Models
{
    public class LlaveArbolNumeroDR
    {
        public Int64 NumeroDR { get; set; }
        public Int64 CodigoHash { get; set; }
        public static int Compare_Llave_Arbol(LlaveArbolNumeroDR x, LlaveArbolNumeroDR y) 
        {
            int r = x.CodigoHash.CompareTo(y.CodigoHash);
            return r;
        }
        public static int Compare_Llave_Arbol2(LlaveArbolNumeroDR x, LlaveArbolNumeroDR y)
        {
            int r = x.NumeroDR.CompareTo(y.NumeroDR);
            return r;
        }
    }
}
