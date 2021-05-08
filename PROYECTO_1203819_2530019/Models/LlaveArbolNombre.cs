using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTO_1203819_2530019.Models
{
    public class LlaveArbolNombre
    {
        public string Nombre { get; set; }
        public Int64 CodigoHash { get; set; }
        public static int Compare_Llave_Arbol(LlaveArbolNombre x, LlaveArbolNombre y)
        {
            int r = x.CodigoHash.CompareTo(y.CodigoHash);
            return r;
        }
        public static int Compare_Llave_Arbol2(LlaveArbolNombre x, LlaveArbolNombre y)
        {
            if (x.Nombre == null)
            {
                return 3;
            }
            int r = x.Nombre.CompareTo(y.Nombre);
            return r;
        }

    }
}
