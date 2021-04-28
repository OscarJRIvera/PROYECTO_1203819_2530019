using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PROYECTO_1203819_2530019.Models;

namespace PROYECTO_1203819_2530019.Data
{
    public class PROYECTO_1203819_2530019Context : DbContext
    {
        public PROYECTO_1203819_2530019Context (DbContextOptions<PROYECTO_1203819_2530019Context> options)
            : base(options)
        {
        }

        public DbSet<PROYECTO_1203819_2530019.Models.Paciente> Paciente { get; set; }

        public DbSet<PROYECTO_1203819_2530019.Models.PacienteView> PacienteView { get; set; }
    }
}
