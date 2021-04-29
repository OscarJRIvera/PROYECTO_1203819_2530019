using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubleLinkedListLibrary1;
using ArbolBinario;
using ArbolDePrioridad;
using TablaHash;

namespace PROYECTO_1203819_2530019.Models.Data
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public AVL<LlaveArbolNombre> Arbol_Nombre;
        public AVL<LlaveArbolNumeroDR> Arbol_NumeroDR;
        public AVL<LlaveArbolApellido> Arbol_Apellido;
        public ArbolDePrioridad<LlaveArbolPrioridad> Arbol_Prioridad;
        public ArbolDePrioridad<LlaveArbolPrioridad> ListaVacunar;
        public DoubleLinkedList<PacienteView> ViewPaciente;
        public TablaHash<string, Paciente> Tabla_Hash;

        private Singleton()
        {
            Tabla_Hash = new TablaHash<string, Paciente>(20, Paciente.Compare_DPI);
            Arbol_Nombre = new AVL<LlaveArbolNombre>(LlaveArbolNombre.Compare_Llave_Arbol, LlaveArbolNombre.Compare_Llave_Arbol2);
            Arbol_NumeroDR = new AVL<LlaveArbolNumeroDR>(LlaveArbolNumeroDR.Compare_Llave_Arbol, LlaveArbolNumeroDR.Compare_Llave_Arbol2);
            Arbol_Apellido = new AVL<LlaveArbolApellido>(LlaveArbolApellido.Compare_Llave_Arbol, LlaveArbolApellido.Compare_Llave_Arbol2);
            Arbol_Prioridad = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            ListaVacunar = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            ViewPaciente = new DoubleLinkedList<PacienteView>();
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
