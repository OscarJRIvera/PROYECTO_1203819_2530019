using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PROYECTO_1203819_2530019.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public int DPI { get; set; }
        [Required]
        public string Departamento { get; set; }
        [Required]
        public string Municipio { get; set; }
        [Required]
        public int Edad { get; set; }
        [Required]
        public int Areadetrabajo { get; set; }
        [Required]
        public int Salud { get; set; }
        [Required]
        public int Est { get; set; }
        [Required]
        public int Asilo { get; set; }

        public static int Compare_Nombre(Paciente x, string y)
        {
            int r = x.Nombre.CompareTo(y);
            return r;
        }
        public static int Compare_DPI(Paciente x, int y)
        {
            int r = x.DPI.CompareTo(y);
            return r;
        }

    }
}
