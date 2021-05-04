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
        public DoubleLinkedList<Paciente> ViewPaciente;//lista de espera/view;
        public DoubleLinkedList<Paciente> Pacientesvacunados; //lista de vacunados
        public DoubleLinkedList<Hospitales> hospitales;
        public TablaHash<string, Paciente> Tabla_Hash;
        public int CantidadPasar;
        public string municipio;
        public int vacunados;
        public int total;
        private Singleton()
        {
            Tabla_Hash = new TablaHash<string, Paciente>(40, Paciente.Compare_DPI);
            Arbol_Nombre = new AVL<LlaveArbolNombre>(LlaveArbolNombre.Compare_Llave_Arbol, LlaveArbolNombre.Compare_Llave_Arbol2);
            Arbol_NumeroDR = new AVL<LlaveArbolNumeroDR>(LlaveArbolNumeroDR.Compare_Llave_Arbol, LlaveArbolNumeroDR.Compare_Llave_Arbol2);
            Arbol_Apellido = new AVL<LlaveArbolApellido>(LlaveArbolApellido.Compare_Llave_Arbol, LlaveArbolApellido.Compare_Llave_Arbol2);
            ViewPaciente = new DoubleLinkedList<Paciente>();
            Pacientesvacunados = new DoubleLinkedList<Paciente>();
            CantidadPasar = 3;
            hospitales = new DoubleLinkedList<Hospitales>();
            municipio = "";
            vacunados = 0;
            total = 0;
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
