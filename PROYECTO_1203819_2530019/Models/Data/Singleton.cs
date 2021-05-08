using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubleLinkedListLibrary1;
using ArbolBinario;
using ArbolDePrioridad;
using TablaHash;
using System.IO;

namespace PROYECTO_1203819_2530019.Models.Data
{
    public class Singleton
    {
        public string GetFolder()
        {
            string mydocs = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = "ProyectoF";
            return mydocs + '\\' + folder;

        }
        private readonly static Singleton _instance = new Singleton();
        public AVL<LlaveArbolNombre> Arbol_Nombre;
        public AVL<LlaveArbolNumeroDR> Arbol_NumeroDR;
        public AVL<LlaveArbolApellido> Arbol_Apellido;
        public DoubleLinkedList<Hospitales> hospitales;
        public TablaHash<string, Paciente> Tabla_Hash;
        public int vacunados;
        public int total;
        public int verificar;
        private Singleton()
        {
            Tabla_Hash = new TablaHash<string, Paciente>(40, Paciente.Compare_DPI);
            Arbol_Nombre = new AVL<LlaveArbolNombre>(LlaveArbolNombre.Compare_Llave_Arbol2);
            Arbol_NumeroDR = new AVL<LlaveArbolNumeroDR>(LlaveArbolNumeroDR.Compare_Llave_Arbol2);
            Arbol_Apellido = new AVL<LlaveArbolApellido>(LlaveArbolApellido.Compare_Llave_Arbol2);
            hospitales = new DoubleLinkedList<Hospitales>();
            vacunados = 0;
            total = 0;
            verificar = 0;
            string Tabla = "\\Tabla.txt";
            if (!Directory.Exists(GetFolder()))
            {
                Directory.CreateDirectory(GetFolder());

            }
            if (!File.Exists(GetFolder() + Tabla))
            {
                var myfile = File.Create(GetFolder() + Tabla);
                myfile.Close();
            }
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
