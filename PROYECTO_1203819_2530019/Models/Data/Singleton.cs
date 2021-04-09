using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbolBinario;

namespace PROYECTO_1203819_2530019.Models.Data
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public AVL<LlaveArbolNombre> Arbol_Nombre;
        public AVL<LlaveArbolNumeroDR> Arbol_NumeroDR;
        public AVL<LlaveArbolApellido> Arbol_Apellido;
        //public Prioridad<LlaveArbolApellido> Arbol_Prioridad;
        private Singleton()
        {
            Arbol_Nombre = new AVL<LlaveArbolNombre>(LlaveArbolNombre.Compare_Llave_Arbol);
            Arbol_NumeroDR = new AVL<LlaveArbolNumeroDR>(LlaveArbolNumeroDR.Compare_Llave_Arbol);
            Arbol_Apellido = new AVL<LlaveArbolApellido>(LlaveArbolApellido.Compare_Llave_Arbol);
        }
        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
