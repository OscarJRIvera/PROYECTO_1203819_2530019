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
        public IActionResult Index()
        {
            return View(F.ViewPaciente);
        }
        public delegate int PacienteComp(LlaveArbolNombre a, string b);
        public delegate int PacienteComp2(LlaveArbolApellido a, string b);
        public delegate int PacienteComp3(LlaveArbolNumeroDR a, string b);
        public IActionResult Registro(int? Id)
        {
            if (F.Busqueda != false)
            {
                F.Arbol_Apellido.Empty();
                F.Arbol_Nombre.Empty();
                F.Arbol_NumeroDR.Empty();
                F.Busqueda = false;
            }
            return View();
        }
        [HttpPost]
        public IActionResult Registro(int id, [Bind("Nombre,Apellido,DPI,Departamento,Municipio,Edad,Areadetrabajo,Salud,Est,Asilo")] Paciente DatosP, [Bind("Nombre,Apellido,DPI,Departamento,Municipio,Edad")] PacienteView Pv1)
        {
            var TempNombre = new LlaveArbolNombre();
            var TempDR = new LlaveArbolNumeroDR();
            var TempApellido = new LlaveArbolApellido();
            //////////////////////////////////////////////////////////////////////////////////////
            TempDR.NumeroDR = DatosP.DPI;
            TempDR.CodigoHash = DatosP.DPI;
            TempDR = F.Arbol_NumeroDR.Find(TempDR);
            if (TempDR == null)
            {
                TempNombre = new LlaveArbolNombre();
                TempDR = new LlaveArbolNumeroDR();
                TempApellido = new LlaveArbolApellido();

                TempDR.CodigoHash = DatosP.DPI;
                TempDR.NumeroDR = DatosP.DPI;
                F.Arbol_NumeroDR.Add(TempDR);

                TempNombre.CodigoHash = DatosP.DPI;
                TempNombre.Nombre = DatosP.Nombre;
                F.Arbol_Nombre.Add(TempNombre);

                TempApellido.CodigoHash = DatosP.DPI;
                TempApellido.Apellido = DatosP.Apellido;
                F.Arbol_Apellido.Add(TempApellido);
            }
            else
            {
                return RedirectToAction("ErrorDPI");
            }
            bool Comprobacion = false;
            LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
            Temp.CodigoHash = DatosP.DPI;
            if (DatosP.Areadetrabajo > 2)
            {
                if (DatosP.Est == 1)
                {
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
                        if (DatosP.Edad >= 70)
                        {
                            Temp.Prioridad = 2.0;
                        }
                        else if (DatosP.Edad >= 50)
                        {
                            Temp.Prioridad = 2.1;
                        }
                        else if (DatosP.Edad >= 40)
                        {
                            Temp.Prioridad = 4.0;
                        }
                        else
                        {
                            Temp.Prioridad = 4.1;
                        }
                        break;
                }
            }
            F.Arbol_Prioridad.add(Temp);
            F.Tabla_Hash.Add(Convert.ToString(DatosP.DPI), DatosP);
            ///////////////////////////////////
            ArbolDePrioridad<LlaveArbolPrioridad> Temparbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            Temparbol = F.Arbol_Prioridad.Clone();
            LlaveArbolPrioridad Tempprioridad;
            F.ViewPaciente = new DoubleLinkedList<Paciente>();
            do
            {
                Tempprioridad = new LlaveArbolPrioridad();
                if (!Temparbol.isempty())
                {
                    Tempprioridad = Temparbol.Remove();
                    if (Tempprioridad != null)
                    {
                        Paciente TempPaciente = new Paciente();
                        TempPaciente = F.Tabla_Hash.Find(Convert.ToString(Tempprioridad.CodigoHash));
                        TempPaciente.Prioridad = Tempprioridad.Prioridad;
                        F.ViewPaciente.Add(TempPaciente);
                    }
                }
            } while (Tempprioridad.CodigoHash != 0);
            return RedirectToAction("Index", "Pacientes");
        }
        public ActionResult Search(string Filter, string Param)
        {
            LlaveArbolNombre ANombre;
            LlaveArbolApellido AApellido;
            LlaveArbolNumeroDR ANumero;
            DoubleLinkedList<Paciente> TempVista = new DoubleLinkedList<Paciente>();
            switch (Filter)
            {
                case "Nombre":
                    AVL<LlaveArbolNombre> Temparbol3 = new AVL<LlaveArbolNombre>(LlaveArbolNombre.Compare_Llave_Arbol, LlaveArbolNombre.Compare_Llave_Arbol2);
                    Temparbol3 = F.Arbol_Nombre.Clone();
                    try
                    {

                        ANombre = new LlaveArbolNombre { Nombre = Param };
                        do
                        {
                            ANombre = Temparbol3.Find2(ANombre);
                            if (ANombre != null)
                            {
                                Temparbol3.RemoveAt(ANombre);
                                Paciente TempPaciente = new Paciente();
                                TempPaciente = F.Tabla_Hash.Find(Convert.ToString(ANombre.CodigoHash));
                                TempVista.Add(TempPaciente);
                            }
                        } while (ANombre != null);
                        return View(TempVista);
                    }
                    catch
                    {
                        return RedirectToAction("ErrorBuscar");
                    }
                case "Apellido":
                    AVL<LlaveArbolApellido> Temparbol2 = new AVL<LlaveArbolApellido>(LlaveArbolApellido.Compare_Llave_Arbol, LlaveArbolApellido.Compare_Llave_Arbol2);
                    Temparbol2 = F.Arbol_Apellido.Clone();
                    try
                    {

                        AApellido = new LlaveArbolApellido { Apellido = Param };
                        do
                        {
                            AApellido = Temparbol2.Find2(AApellido);
                            if (AApellido != null)
                            {
                                Temparbol2.RemoveAt(AApellido);
                                Paciente TempPaciente = new Paciente();
                                TempPaciente = F.Tabla_Hash.Find(Convert.ToString(AApellido.CodigoHash));
                                TempVista.Add(TempPaciente);
                            }
                        } while (AApellido != null);
                        return View(TempVista);
                    }
                    catch
                    {
                        return RedirectToAction("ErrorBuscar");
                    }
                case "DPI":
                    AVL<LlaveArbolNumeroDR> Temparbol = new AVL<LlaveArbolNumeroDR>(LlaveArbolNumeroDR.Compare_Llave_Arbol, LlaveArbolNumeroDR.Compare_Llave_Arbol2);
                    Temparbol = F.Arbol_NumeroDR.Clone();
                    try
                    {


                        Int64 Param2 = Convert.ToInt64(Param);
                        ANumero = new LlaveArbolNumeroDR { NumeroDR = Param2 };
                        do
                        {
                            ANumero = Temparbol.Find2(ANumero);
                            if (ANumero != null)
                            {
                                Temparbol.RemoveAt(ANumero);
                                Paciente TempPaciente = new Paciente();
                                TempPaciente = F.Tabla_Hash.Find(Convert.ToString(ANumero.CodigoHash));
                                TempVista.Add(TempPaciente);
                            }
                        } while (ANumero != null);
                        return View(TempVista);
                    }
                    catch
                    {
                        return RedirectToAction("ErrorBuscar");
                    }

                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult ErrorBuscar()
        {
            return View();
        }
        public IActionResult ErrorDPI()
        {
            return View();
        }
        public IActionResult Vacunacion()
        {
            restablecerfechas();
            if (!F.Arbol_Prioridad.isempty())
            {
                F.ArbolVacunar = F.Arbol_Prioridad.Clone();
                F.Arbol_Prioridad.Empty();
            }
            return RedirectToAction("Vacunacion2");
        }
        public IActionResult Vacunacion2()
        {
            if (!F.ArbolVacunarNollegados.isempty())
            {
                restablecerfechas2();
            }
            DoubleLinkedList<Paciente> TempView = new DoubleLinkedList<Paciente>();
            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol2 = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            if (F.ArbolVacunar.isempty() && F.ArbolVacunarNollegados.isempty())
            {
                return View(TempView);
            }
            if (!F.ArbolVacunar.isempty())
            {
                TempArbol = F.ArbolVacunar.Clone();
                do
                {
                    LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
                    Temp = TempArbol.Remove();
                    Paciente ViewVacunar = new Paciente();
                    ViewVacunar = F.Tabla_Hash.Find(Convert.ToString(Temp.CodigoHash));
                    TempView.Add(ViewVacunar);
                } while (!TempArbol.isempty());
            }
            if (F.ArbolVacunarNollegados.isempty())
            {
                return View(TempView);
            }
            TempArbol2 = F.ArbolVacunarNollegados.Clone();
            do
            {
                LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
                Temp = TempArbol2.Remove();
                Paciente ViewVacunar = new Paciente();
                ViewVacunar = F.Tabla_Hash.Find(Convert.ToString(Temp.CodigoHash));
                TempView.Add(ViewVacunar);
            } while (!TempArbol2.isempty());
            return View(TempView);
        }
        public bool CalcSiLlego()
        {
            Random Rand = new Random();
            int numero = Rand.Next(1, 4);
            if (numero == 2)
            {
                return false;
            }
            return true;

        }
        public void restablecerfechas()
        {
            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            F.TempFecha = new DateTime();
            F.TempFecha = DateTime.Today;
            TempArbol = F.Arbol_Prioridad.Clone();
            F.TempFecha = F.TempFecha.AddDays(7);
            F.TempFecha = F.TempFecha.AddHours(8);
            while (!TempArbol.isempty())
            {
                for (int i = 0; i < F.CantidadPasar; i++)
                {
                    if (!TempArbol.isempty())
                    {
                        LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
                        Temp = TempArbol.Remove();
                        Paciente ViewVacunar = new Paciente();
                        ViewVacunar = F.Tabla_Hash.Find(Convert.ToString(Temp.CodigoHash));
                        F.Tabla_Hash.Remove(Convert.ToString(Temp.CodigoHash));
                        ViewVacunar.Fecha = F.TempFecha;
                        F.Tabla_Hash.Add(Convert.ToString(ViewVacunar.DPI), ViewVacunar);
                    }
                }
                F.TempFecha = F.TempFecha.AddMinutes(15);
            }
        }
        public void restablecerfechas2()
        {
            DateTime Tempfecha2 = new DateTime();
            Tempfecha2 = F.TempFecha;
            DateTime Tempfecha3 = new DateTime();
            int Repetidos = 0;
            if (F.ArbolVacunarNollegados.isempty())
            {
                return;
            }
            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            TempArbol = F.ArbolVacunarNollegados.Clone();
            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol2 = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            if (F.ArbolVacunar.isempty())
            {
                Repetidos = F.CantidadPasar;
            }
            else
            {
                TempArbol2 = F.ArbolVacunar.Clone();
                while (!TempArbol2.isempty())
                {
                    LlaveArbolPrioridad llaveA = TempArbol2.Remove();
                    Paciente Tpaciente = F.Tabla_Hash.Find(Convert.ToString(llaveA.CodigoHash));
                    if (Tpaciente.Fecha.AddMinutes(15) == F.TempFecha)
                    {
                        Tempfecha3 = Tpaciente.Fecha;
                        Repetidos++;
                    }
                }
            }
            if (Repetidos == F.CantidadPasar)
            {
                while (!TempArbol.isempty())
                {
                    for (int i = 0; i < F.CantidadPasar; i++)
                    {
                        if (!TempArbol.isempty())
                        {
                            LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
                            Temp = TempArbol.Remove();
                            Paciente ViewVacunar = new Paciente();
                            ViewVacunar = F.Tabla_Hash.Find(Convert.ToString(Temp.CodigoHash));
                            F.Tabla_Hash.Remove(Convert.ToString(Temp.CodigoHash));
                            ViewVacunar.Fecha = Tempfecha2;
                            F.Tabla_Hash.Add(Convert.ToString(ViewVacunar.DPI), ViewVacunar);
                        }
                    }
                    Tempfecha2 = Tempfecha2.AddMinutes(15);
                }
            }
            else
            {
                for (int h = Repetidos; h < F.CantidadPasar; h++)
                {
                    if (TempArbol.isempty())
                    {
                        LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
                        Temp = TempArbol.Remove();
                        Paciente ViewVacunar = new Paciente();
                        ViewVacunar = F.Tabla_Hash.Find(Convert.ToString(Temp.CodigoHash));
                        F.Tabla_Hash.Remove(Convert.ToString(Temp.CodigoHash));
                        ViewVacunar.Fecha = Tempfecha3;
                        F.Tabla_Hash.Add(Convert.ToString(ViewVacunar.DPI), ViewVacunar);
                    }

                }
                while (!TempArbol.isempty())
                {
                    for (int i = 0; i < F.CantidadPasar; i++)
                    {
                        if (!TempArbol.isempty())
                        {
                            LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
                            Temp = TempArbol.Remove();
                            Paciente ViewVacunar = new Paciente();
                            ViewVacunar = F.Tabla_Hash.Find(Convert.ToString(Temp.CodigoHash));
                            F.Tabla_Hash.Remove(Convert.ToString(Temp.CodigoHash));
                            ViewVacunar.Fecha = Tempfecha2;
                            F.Tabla_Hash.Add(Convert.ToString(ViewVacunar.DPI), ViewVacunar);
                        }
                    }
                    Tempfecha2 = Tempfecha2.AddMinutes(15);
                }
            }

        }
        public IActionResult Realizarinjeccion()
        {
            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol2 = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            if (F.ArbolVacunar.isempty() && F.ArbolVacunarNollegados.isempty())
            {
                return RedirectToAction("Vacunacion2");
            }
            if (!F.ArbolVacunar.isempty())
            {
                TempArbol = F.ArbolVacunar.Clone();
            }
            if (!F.ArbolVacunarNollegados.isempty())
            {
                TempArbol2 = F.ArbolVacunarNollegados.Clone();
            }
            DoubleLinkedList<Paciente> ViewVacunacion = new DoubleLinkedList<Paciente>();
            for (int i = 0; i < F.CantidadPasar; i++)
            {
                if (!TempArbol.isempty())
                {
                    LlaveArbolPrioridad Templlave = new LlaveArbolPrioridad();
                    Templlave = TempArbol.Remove();
                    Paciente ViewVacunar = new Paciente();
                    ViewVacunar = F.Tabla_Hash.Find(Convert.ToString(Templlave.CodigoHash));
                    ViewVacunacion.Add(ViewVacunar);
                }
                else if (!TempArbol2.isempty())
                {
                    LlaveArbolPrioridad Templlave = new LlaveArbolPrioridad();
                    Templlave = TempArbol2.Remove();
                    Paciente ViewVacunar = new Paciente();
                    ViewVacunar = F.Tabla_Hash.Find(Convert.ToString(Templlave.CodigoHash));
                    ViewVacunacion.Add(ViewVacunar);
                }
            }
            return View(ViewVacunacion);
        }
        public IActionResult Realizarinjeccion2()
        {
            DoubleLinkedList<Paciente> ViewEspera = new DoubleLinkedList<Paciente>();
            ArbolDePrioridad<LlaveArbolPrioridad> TempNollegados = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            TempNollegados = F.ArbolVacunarNollegados.Clone();
            for (int i = 0; i < F.CantidadPasar; i++)
            {
                if (!F.ArbolVacunar.isempty() || !TempNollegados.isempty())
                {
                    LlaveArbolPrioridad Templlave = new LlaveArbolPrioridad();
                    if (!F.ArbolVacunar.isempty())
                    {
                        Templlave = F.ArbolVacunar.Remove();
                    }
                    else
                    {
                        Templlave = F.ArbolVacunarNollegados.Remove();
                        TempNollegados.Remove();
                    }


                    Paciente Temppaciente = new Paciente();
                    Temppaciente = F.Tabla_Hash.Find(Convert.ToString(Templlave.CodigoHash));
                    if (CalcSiLlego())
                    {
                        F.Tabla_Hash.Remove(Convert.ToString(Templlave.CodigoHash));
                        Temppaciente.Paciente_Llego = true;
                    }
                    else
                    {
                        Temppaciente.Paciente_Llego = false;
                        F.ArbolVacunarNollegados.add(Templlave);
                    }
                    ViewEspera.Add(Temppaciente);
                }

            }

            return View(ViewEspera);
        }
        public IActionResult RegesoRegistrar()
        {
            if (!F.ArbolVacunar.isempty())
            {
                return RedirectToAction("Vacunacion2");
            }
            F.ViewPaciente = new DoubleLinkedList<Paciente>();
            F.Busqueda = true;
            return RedirectToAction("Index");
        }
    }
}

