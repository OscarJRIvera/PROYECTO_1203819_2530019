using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTO_1203819_2530019.Models
{
    public class PacienteView
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public Int64 DPI { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public int Edad { get; set; }
        public double Prioridad { get; set; }
        public DateTime Fecha { get; set; }
    }
}
