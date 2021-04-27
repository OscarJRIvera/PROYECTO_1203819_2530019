using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROYECTO_1203819_2530019.Data;
using PROYECTO_1203819_2530019.Models;
using DoubleLinkedListLibrary1;
using ArbolBinario;
using ArbolDePrioridad;
namespace PROYECTO_1203819_2530019.Controllers
{
    public class PacientesController : Controller
    {
        private readonly Models.Data.Singleton F = Models.Data.Singleton.Instance;

        private readonly PROYECTO_1203819_2530019Context _context;

        public PacientesController(PROYECTO_1203819_2530019Context context)
        {
            _context = context;
        }

        // GET: Pacientes
        public IActionResult Index()
        {
            return View(F.ViewPaciente);
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,DPI,Departamente,Municipio,Edad")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,DPI,Departamente,Municipio,Edad")] Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Paciente.FindAsync(id);
            _context.Paciente.Remove(paciente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Paciente.Any(e => e.Id == id);
        }
        public IActionResult Registro(int? Id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registro(int id, [Bind("Nombre,Apellido,DPI,Departamento,Municipio,Edad,Areadetrabajo,Salud,Est,Asilo")] Paciente DatosP, [Bind("Nombre,Apellido,DPI,Departamento,Municipio,Edad")] PacienteView Pv1)
        {
            bool Comprobacion = false;
            LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
            F.ViewPaciente.Add(Pv1);
            if (DatosP.Areadetrabajo > 2) {
                if (DatosP.Est == 1) {
                    Temp.Prioridad = 1.2;
                    Comprobacion = true;
                }
                if (DatosP.Areadetrabajo > 5)
                {
                    if (DatosP.Asilo == 1)
                    {
                        Temp.Prioridad = 1.4;
                        Comprobacion = true;
                    }
                    if (DatosP.Areadetrabajo > 6)
                    {
                        if (DatosP.Salud != 10)
                        {
                            Temp.Prioridad = 2.0;
                            Comprobacion = true;
                        }
                    }
                }
            }
            if (Comprobacion != true)
            {
                switch (DatosP.Areadetrabajo)
                {
                    case 1:
                        Temp.Prioridad = 1.0; break;
                    case 2:
                        Temp.Prioridad = 1.1; break;
                    case 3:
                        Temp.Prioridad = 1.3; break;
                    case 4:
                        Temp.Prioridad = 1.3; break;
                    case 5:
                        Temp.Prioridad = 1.3; break;
                    case 6:
                        Temp.Prioridad = 1.5; break;
                    case 7:
                        Temp.Prioridad = 3.0; break;
                    case 8:
                        Temp.Prioridad = 3.1; break;
                    case 9:
                        Temp.Prioridad = 3.2; break;
                    case 10:
                        Temp.Prioridad = 3.3; break;
                    case 11:
                        if (DatosP.Edad >= 70 ) {
                            Temp.Prioridad = 2.0;
                        }
                        else if (DatosP.Edad >= 50){
                            Temp.Prioridad = 2.1;
                        }
                        else if (DatosP.Edad >= 40){
                            Temp.Prioridad = 4.0;
                        }
                        else { 
                            Temp.Prioridad = 4.1;
                        } break;
                }
            }
            
            F.Arbol_Prioridad.add(Temp);
            return RedirectToAction("Index", "Pacientes");
        }
        public delegate int PacienteComp(LlaveArbolNombre a, LlaveArbolNombre b);
        public delegate int PacienteComp2(LlaveArbolApellido a, LlaveArbolApellido b);
        public delegate int PacienteComp3(LlaveArbolNumeroDR a, LlaveArbolNumeroDR b);
        public ActionResult Search(string Filter, string Param)
        {
            PacienteComp comparador;
            PacienteComp2 comparador2;
            PacienteComp3 comparador3;
            LlaveArbolNombre ANombre;
            LlaveArbolApellido AApellido;
            LlaveArbolNumeroDR ANumero;

           switch (Filter)
           {
             case "Nombre":
                    comparador = LlaveArbolNombre.Compare_Llave_Arbol;
                    ANombre = new LlaveArbolNombre { Nombre = Param };
                    
                    break;
             case "Apellido":
                    comparador2 = LlaveArbolApellido.Compare_Llave_Arbol;
                    AApellido = new LlaveArbolApellido { Apellido = Param };
                    break;
             case "DPI":
                    comparador3 = LlaveArbolNumeroDR.Compare_Llave_Arbol;
                    ANumero = new LlaveArbolNumeroDR { NumeroDR = Param };
                    break;
                default:
                    break;
           }
            return View();
        }
    }
}
