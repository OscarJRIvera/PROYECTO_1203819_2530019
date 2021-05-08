using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArbolDePrioridad;
using TablaHash;

namespace PROYECTO_1203819_2530019.Models
{
    public class Hospitales
    {
        public int Id { get; set; }
        public string Municipio { get; set; }
        public DateTime TempFecha = new DateTime();
        public int CantidadPasar = 3;

        public ArbolDePrioridad<LlaveArbolPrioridad> Registrar = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
        public ArbolDePrioridad<LlaveArbolPrioridad> vacunados = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
        public ArbolDePrioridad<LlaveArbolPrioridad> Espera = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
        public TablaHash<string, Paciente> Pacientes = new TablaHash<string, Paciente>(20, Paciente.Compare_DPI);
        public TablaHash<string, Paciente> PacientesVacunados = new TablaHash<string, Paciente>(20, Paciente.Compare_DPI);
        public static int Compare_Municipio(Hospitales x, string y)
        {
            int r = x.Municipio.CompareTo(y);
            return r;
        }
    }
}
