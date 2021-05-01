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

        public ArbolDePrioridad<LlaveArbolPrioridad> prioridad = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
        
        public TablaHash<string, Paciente> Pacientes = new TablaHash<string, Paciente>(20, Paciente.Compare_DPI);
    }
}
