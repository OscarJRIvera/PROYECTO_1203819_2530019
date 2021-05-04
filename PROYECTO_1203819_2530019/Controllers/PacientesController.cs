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
            var ViewPaciente = new DoubleLinkedList<Paciente>();
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            Hospitales hospital = new Hospitales();
            hospital = F.hospitales.Find(m => Comparador(m, F.municipio) == 0);
            ArbolDePrioridad<LlaveArbolPrioridad> Temparbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            if (hospital.Registrar.isempty())
            {
                return View(ViewPaciente);
            }
            Temparbol = hospital.Registrar.Clone();
            LlaveArbolPrioridad Tempprioridad;
            do
            {
                Tempprioridad = new LlaveArbolPrioridad();
                if (!Temparbol.isempty())
                {
                    Tempprioridad = Temparbol.Remove();
                    if (Tempprioridad != null)
                    {
                        Paciente TempPaciente = new Paciente();
                        TempPaciente = hospital.Pacientes.Find(Convert.ToString(Tempprioridad.CodigoHash));
                        TempPaciente.Prioridad = Tempprioridad.Prioridad;
                        ViewPaciente.Add(TempPaciente);
                    }
                }
            } while (Tempprioridad.CodigoHash != 0);
            return View(ViewPaciente);
        }
        public delegate int PacienteComp(Hospitales a, string b);
        public IActionResult Registro(int? Id)
        {

            return View();
        }
        [HttpPost]
        public IActionResult Registro(int id, [Bind("Nombre,Apellido,DPI,Departamento,Municipio,Edad,Areadetrabajo,Salud,Est,Asilo")] Paciente DatosP)
        {
            var TempNombre = new LlaveArbolNombre();
            var TempDR = new LlaveArbolNumeroDR();
            var TempApellido = new LlaveArbolApellido();
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
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            Hospitales hospital = new Hospitales();
            hospital = F.hospitales.Find(m => Comparador(m, DatosP.Municipio) == 0);
            if (hospital == null)
            {
                Hospitales Nuevohospital = new Hospitales();
                Nuevohospital.Municipio = DatosP.Municipio;
                F.hospitales.Add(Nuevohospital);
                hospital = F.hospitales.Find(m => Comparador(m, DatosP.Municipio) == 0);
            }
            var arbolP = hospital.Registrar;
            var tablah = hospital.Pacientes;
            F.total++;
            arbolP.add(Temp);
            tablah.Add(Convert.ToString(DatosP.DPI), DatosP);
            F.Tabla_Hash.Add(Convert.ToString(DatosP.DPI), DatosP);
            F.municipio = DatosP.Municipio;
            return RedirectToAction("Index", "Pacientes");
        }
        public ActionResult Search(string Filter, string Param)
        {
            if (Param == null)
            {
                return RedirectToAction("ErrorBuscar");
            }
            LlaveArbolNombre ANombre;
            LlaveArbolApellido AApellido;
            LlaveArbolNumeroDR ANumero;
            DoubleLinkedList<Paciente> TempVista = new DoubleLinkedList<Paciente>();
            switch (Filter)
            {
                case "Nombre":
                    try
                    {
                        Int64 pureba1 = Convert.ToInt64(Param);
                        return RedirectToAction("ErrorBuscar");
                    }
                    catch
                    { }
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
                    try
                    {
                        Int64 pureba1 = Convert.ToInt64(Param);
                        return RedirectToAction("ErrorBuscar");
                    }
                    catch
                    { }
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
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            Hospitales hospital = new Hospitales();
            hospital = F.hospitales.Find(m => Comparador(m, F.municipio) == 0);
            restablecerfechas();
            if (!hospital.Registrar.isempty())
            {
                hospital.Espera = hospital.Registrar.Clone();
                hospital.Registrar.Empty();
            }
            else
            {
                return RedirectToAction("ErrorVacunacion");
            }
            return RedirectToAction("Vacunacion2");
        }
        public IActionResult Vacunacion2()
        {
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            Hospitales hospital = new Hospitales();
            hospital = F.hospitales.Find(m => Comparador(m, F.municipio) == 0);

            restablecerfechas2();
            DoubleLinkedList<Paciente> TempView = new DoubleLinkedList<Paciente>();
            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            if (hospital.Espera.isempty())
            {
                return View(TempView);
            }
            TempArbol = hospital.Espera.Clone();
            do
            {
                LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
                Temp = TempArbol.Remove();
                Paciente ViewVacunar = new Paciente();
                ViewVacunar = hospital.Pacientes.Find(Convert.ToString(Temp.CodigoHash));
                TempView.Add(ViewVacunar);
            } while (!TempArbol.isempty());
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
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            var hospital = F.hospitales.Find(m => Comparador(m, F.municipio) == 0);

            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            hospital.TempFecha = new DateTime();
            hospital.TempFecha = DateTime.Today;
            if (hospital.Registrar.isempty())
            {
                return;
            }
            TempArbol = hospital.Registrar.Clone();
            hospital.TempFecha = hospital.TempFecha.AddDays(7);
            hospital.TempFecha = hospital.TempFecha.AddHours(8);
            while (!TempArbol.isempty())
            {
                for (int i = 0; i < F.CantidadPasar; i++)
                {
                    if (!TempArbol.isempty())
                    {
                        LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
                        Temp = TempArbol.Remove();
                        Paciente ViewVacunar = new Paciente();
                        ViewVacunar = hospital.Pacientes.Find(Convert.ToString(Temp.CodigoHash));
                        hospital.Pacientes.Remove(Convert.ToString(Temp.CodigoHash));
                        ViewVacunar.Fecha = hospital.TempFecha;
                        hospital.Pacientes.Add(Convert.ToString(ViewVacunar.DPI), ViewVacunar);
                    }
                }
                hospital.TempFecha = hospital.TempFecha.AddMinutes(15);
            }
        }
        public void restablecerfechas2()
        {
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            var hospital = F.hospitales.Find(m => Comparador(m, F.municipio) == 0);

            ArbolDePrioridad<LlaveArbolPrioridad> TempArbolvacuna = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            if (!hospital.Espera.isempty())
            {
                TempArbolvacuna = hospital.Espera.Clone();
                LlaveArbolPrioridad TempF = new LlaveArbolPrioridad();
                TempF = TempArbolvacuna.Remove();
                Paciente Nuevo = new Paciente();
                Nuevo = hospital.Pacientes.Find(Convert.ToString(TempF.CodigoHash));
                if (TempF.Prioridad > 4.1)
                {
                    hospital.TempFecha = Nuevo.Fecha.AddMinutes(15);
                }
                else
                {
                    hospital.TempFecha = Nuevo.Fecha;
                }

            }
            else
            {
                return;
            }
            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            TempArbol = hospital.Espera.Clone();
            while (!TempArbol.isempty())
            {
                for (int i = 0; i < F.CantidadPasar; i++)
                {
                    if (!TempArbol.isempty())
                    {
                        LlaveArbolPrioridad Temp = new LlaveArbolPrioridad();
                        Temp = TempArbol.Remove();
                        Paciente ViewVacunar = new Paciente();
                        ViewVacunar = hospital.Pacientes.Find(Convert.ToString(Temp.CodigoHash));
                        hospital.Pacientes.Remove(Convert.ToString(Temp.CodigoHash));
                        ViewVacunar.Fecha = hospital.TempFecha;
                        hospital.Pacientes.Add(Convert.ToString(ViewVacunar.DPI), ViewVacunar);
                    }
                }
                hospital.TempFecha = hospital.TempFecha.AddMinutes(15);
            }
        }
        public IActionResult Realizarinjeccion()
        {
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            Hospitales hospital = new Hospitales();
            hospital = F.hospitales.Find(m => Comparador(m, F.municipio) == 0);

            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            if (hospital.Espera.isempty())
            {
                return RedirectToAction("Vacunacion2");
            }
            TempArbol = hospital.Espera.Clone();
            DoubleLinkedList<Paciente> ViewVacunacion = new DoubleLinkedList<Paciente>();
            for (int i = 0; i < F.CantidadPasar; i++)
            {
                if (!TempArbol.isempty())
                {
                    LlaveArbolPrioridad Templlave = new LlaveArbolPrioridad();
                    Templlave = TempArbol.Remove();
                    Paciente ViewVacunar = new Paciente();
                    ViewVacunar = hospital.Pacientes.Find(Convert.ToString(Templlave.CodigoHash));
                    ViewVacunacion.Add(ViewVacunar);
                }
            }
            return View(ViewVacunacion);
        }
        public IActionResult Realizarinjeccion2()
        {
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            var hospital = F.hospitales.Find(m => Comparador(m, F.municipio) == 0);

            DoubleLinkedList<Paciente> ViewEspera = new DoubleLinkedList<Paciente>();
            ArbolDePrioridad<LlaveArbolPrioridad> PacienteNollego = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            for (int i = 0; i < F.CantidadPasar; i++)
            {
                if (!hospital.Espera.isempty())
                {
                    LlaveArbolPrioridad Templlave = new LlaveArbolPrioridad();
                    Templlave = hospital.Espera.Remove();
                    Paciente Temppaciente = new Paciente();
                    Temppaciente = hospital.Pacientes.Find(Convert.ToString(Templlave.CodigoHash));
                    hospital.Pacientes.Remove(Convert.ToString(Templlave.CodigoHash));
                    if (CalcSiLlego())
                    {
                        Temppaciente.Paciente_Llego = true;
                        hospital.vacunados.add(Templlave);
                        hospital.PacientesVacunados.Add(Convert.ToString(Templlave.CodigoHash), Temppaciente);
                        F.vacunados++;
                    }
                    else
                    {
                        Temppaciente.Paciente_Llego = false;
                        Templlave.Prioridad = Templlave.Prioridad + 4.1;
                        Temppaciente.Prioridad = Templlave.Prioridad + 4.1;
                        hospital.Pacientes.Add(Convert.ToString(Templlave.CodigoHash), Temppaciente);
                        PacienteNollego.add(Templlave);
                    }
                    ViewEspera.Add(Temppaciente);
                }
            }
            while (!PacienteNollego.isempty())
            {
                hospital.Espera.add(PacienteNollego.Remove());
            }
            return View(ViewEspera);
        }
        public IActionResult Buscar()
        {
            DoubleLinkedList<Paciente> vista = new DoubleLinkedList<Paciente>();
            for (int i = 1; i <= F.hospitales.Count2(); i++)
            {
                Hospitales Temphospital = new Hospitales();
                Temphospital = F.hospitales.GetbyIndex(i);
                var Temparbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);

                if (!Temphospital.Registrar.isempty())
                {
                    Temparbol = Temphospital.Registrar.Clone();
                    while (!Temparbol.isempty())
                    {
                        var templlave = new LlaveArbolPrioridad();
                        templlave = Temparbol.Remove();
                        vista.Add(Temphospital.Pacientes.Find(Convert.ToString(templlave.CodigoHash)));
                    }
                }
                if (!Temphospital.vacunados.isempty())
                {
                    Temparbol = Temphospital.vacunados.Clone();
                    while (!Temparbol.isempty())
                    {
                        var templlave = new LlaveArbolPrioridad();
                        templlave = Temparbol.Remove();
                        vista.Add(Temphospital.PacientesVacunados.Find(Convert.ToString(templlave.CodigoHash)));
                    }
                }
            }
            return View(vista);
        }
        public IActionResult Hospital()
        {
            return View();
        }
        public IActionResult ViewHospital()
        {
            return View();
        }
        public IActionResult InfoHospital([Bind("Departamento,Municipio")] Paciente DatosP)
        {
            if (DatosP.Departamento != null)
            {
                F.municipio = DatosP.Municipio;
            }
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            Hospitales hospital = new Hospitales();
            hospital = F.hospitales.Find(m => Comparador(m, F.municipio) == 0);
            if (hospital == null)
            {
                hospital = new Hospitales();
                hospital.Municipio = F.municipio;
                F.hospitales.Add(hospital);
            }
            return View();
        }
        public IActionResult ListaVacunados()
        {
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            Hospitales hospital = new Hospitales();
            hospital = F.hospitales.Find(m => Comparador(m, F.municipio) == 0);
            var TempViewPaciente = new DoubleLinkedList<Paciente>();

            var Temparbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            if (!hospital.vacunados.isempty())
            {
                Temparbol = hospital.vacunados.Clone();
                while (!Temparbol.isempty())
                {
                    var Templlave = new LlaveArbolPrioridad();
                    Templlave = Temparbol.Remove();
                    var Temppaciente = new Paciente();
                    Temppaciente = hospital.PacientesVacunados.Find(Convert.ToString(Templlave.CodigoHash));
                    TempViewPaciente.Add(Temppaciente);
                }
            }


            return View(TempViewPaciente);
        }
        public IActionResult ErrorVacunacion()
        {
            return View();
        }
        public IActionResult CambiarMaximimo([Bind("maximo")] MaximoPasar Tempmaximo)
        {
            F.CantidadPasar = Tempmaximo.maximo;
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Porcentaje()
        {
            Porcentajes Temp = new Porcentajes();
            Temp.Total = F.total;
            Temp.Vacunados = F.vacunados;
            if (F.total == 0)
            {
                Temp.porcentaje = "0%";
                return View(Temp);
            }
            int porcentaje = ((F.vacunados / F.total) * 100);
            Temp.porcentaje = porcentaje + "%";
            return View(Temp);
        }
    }
}

