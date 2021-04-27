using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubleLinkedListLibrary1;
using ArbolBinario;
using ArbolDePrioridad;

namespace PROYECTO_1203819_2530019.Models.Data
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public AVL<LlaveArbolNombre> Arbol_Nombre;
        public AVL<LlaveArbolNumeroDR> Arbol_NumeroDR;
        public AVL<LlaveArbolApellido> Arbol_Apellido;
        public ArbolDePrioridad<LlaveArbolPrioridad> Arbol_Prioridad;
        public DoubleLinkedList<PacienteView> ViewPaciente;
        //public TablaHash<string, Paciente> Tabla_Hash;
        
        private Singleton()
        {
            //Tabla_Hash = new TablaHash<String, Paciente>(20,Paciente.Compare_Nombre);
            Arbol_Nombre = new AVL<LlaveArbolNombre>(LlaveArbolNombre.Compare_Llave_Arbol);
            Arbol_NumeroDR = new AVL<LlaveArbolNumeroDR>(LlaveArbolNumeroDR.Compare_Llave_Arbol);
            Arbol_Apellido = new AVL<LlaveArbolApellido>(LlaveArbolApellido.Compare_Llave_Arbol);
            Arbol_Prioridad = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            ViewPaciente= new DoubleLinkedList<PacienteView>();
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
