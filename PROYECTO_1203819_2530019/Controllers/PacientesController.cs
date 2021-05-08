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
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.VisualBasic.FileIO;
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
            ;
            hospital = F.hospitales.Find(m => Comparador(m, HttpContext.Session.GetString("municipio")) == 0);
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
            if (F.verificar == 0)
            {
                F.verificar = 1;
                Cargardatos();
            }
            return View();
        }
        [HttpPost]
        public IActionResult Registro(int id, [Bind("Nombre,Apellido,DPI,Departamento,Municipio,Edad,Areadetrabajo,Salud,Est,Asilo")] Paciente DatosP)
        {
            var TempNombre = new LlaveArbolNombre();
            var TempDR = new LlaveArbolNumeroDR();
            var TempApellido = new LlaveArbolApellido();
            TempDR.NumeroDR = DatosP.DPI;
            TempDR = F.Arbol_NumeroDR.Find(TempDR);
            TempNombre.Nombre = DatosP.Nombre;
            TempNombre = F.Arbol_Nombre.Find(TempNombre);
            TempApellido.Apellido = DatosP.Apellido;
            TempApellido = F.Arbol_Apellido.Find(TempApellido);
            if (TempDR == null)
            {
                TempDR = new LlaveArbolNumeroDR();
                TempDR.CodigoHash = DatosP.DPI;
                TempDR.NumeroDR = DatosP.DPI;
                F.Arbol_NumeroDR.Add(TempDR);
            }
            else
            {
                return RedirectToAction("ErrorDPI");
            }
            if (TempNombre == null)
            {
                TempNombre = new LlaveArbolNombre();
                TempNombre.CodigoHash = DatosP.DPI;
                TempNombre.Nombre = DatosP.Nombre;
                F.Arbol_Nombre.Add(TempNombre);
            }
            if (TempApellido == null)
            {
                TempApellido = new LlaveArbolApellido();
                TempApellido.CodigoHash = DatosP.DPI;
                TempApellido.Apellido = DatosP.Apellido;
                F.Arbol_Apellido.Add(TempApellido);
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
            DatosP.Prioridad = Temp.Prioridad;
            tablah.Add(Convert.ToString(DatosP.DPI), DatosP);
            F.Tabla_Hash.Add(Convert.ToString(DatosP.DPI), DatosP);
            HttpContext.Session.SetString("municipio", DatosP.Municipio);
            string mydocs = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = "ProyectoF";
            string R = mydocs + '\\' + folder;
            Escribir(R + "\\Tabla.txt", EscribirDatos());
            return RedirectToAction("Index", "Pacientes");
        }
        public ActionResult Search(string Filter, string Param)
        {
            if (F.Arbol_Nombre.isempty())
            {
                return RedirectToAction("ErrorBuscar");
            }
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
                    AVL<LlaveArbolNombre> Temparbol3 = new AVL<LlaveArbolNombre>(LlaveArbolNombre.Compare_Llave_Arbol2);
                    Temparbol3 = F.Arbol_Nombre.Clone();
                    try
                    {
                        ANombre = new LlaveArbolNombre { Nombre = Param };
                        ANombre = Temparbol3.Find(ANombre);
                        if (ANombre != null)
                        {
                            Temparbol3.RemoveAt(ANombre);
                            Paciente TempPaciente = new Paciente();
                            TempPaciente = F.Tabla_Hash.Find(Convert.ToString(ANombre.CodigoHash));
                            TempVista.Add(TempPaciente);
                        }
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
                    AVL<LlaveArbolApellido> Temparbol2 = new AVL<LlaveArbolApellido>(LlaveArbolApellido.Compare_Llave_Arbol2);
                    Temparbol2 = F.Arbol_Apellido.Clone();
                    try
                    {
                        AApellido = new LlaveArbolApellido { Apellido = Param };
                        AApellido = Temparbol2.Find(AApellido);
                        if (AApellido != null)
                        {
                            Temparbol2.RemoveAt(AApellido);
                            Paciente TempPaciente = new Paciente();
                            TempPaciente = F.Tabla_Hash.Find(Convert.ToString(AApellido.CodigoHash));
                            TempVista.Add(TempPaciente);
                        }
                        return View(TempVista);
                    }
                    catch
                    {
                        return RedirectToAction("ErrorBuscar");
                    }
                case "DPI":
                    AVL<LlaveArbolNumeroDR> Temparbol = new AVL<LlaveArbolNumeroDR>(LlaveArbolNumeroDR.Compare_Llave_Arbol2);
                    Temparbol = F.Arbol_NumeroDR.Clone();
                    try
                    {
                        Int64 Param2 = Convert.ToInt64(Param);
                        ANumero = new LlaveArbolNumeroDR { NumeroDR = Param2 };
                        ANumero = Temparbol.Find(ANumero);
                        if (ANumero != null)
                        {
                            Temparbol.RemoveAt(ANumero);
                            Paciente TempPaciente = new Paciente();
                            TempPaciente = F.Tabla_Hash.Find(Convert.ToString(ANumero.CodigoHash));
                            TempVista.Add(TempPaciente);
                        }
                        return View(TempVista);
                    }
                    catch
                    {
                        return RedirectToAction("ErrorBuscar");
                    }

                default:
                    return RedirectToAction("Busqueda");
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
            hospital = F.hospitales.Find(m => Comparador(m, HttpContext.Session.GetString("municipio")) == 0);
            if (!hospital.Espera.isempty())
            {
                return RedirectToAction("ErrorRegVac");
            }
            restablecerfechas();
            string mydocs = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = "ProyectoF";
            string R = mydocs + '\\' + folder;
            Escribir(R + "\\Tabla.txt", EscribirDatos());
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
            hospital = F.hospitales.Find(m => Comparador(m, HttpContext.Session.GetString("municipio")) == 0);

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
            var hospital = F.hospitales.Find(m => Comparador(m, HttpContext.Session.GetString("municipio")) == 0);

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
                for (int i = 0; i < hospital.CantidadPasar; i++)
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
            var hospital = F.hospitales.Find(m => Comparador(m, HttpContext.Session.GetString("municipio")) == 0);

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
                for (int i = 0; i < hospital.CantidadPasar; i++)
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
            hospital = F.hospitales.Find(m => Comparador(m, HttpContext.Session.GetString("municipio")) == 0);

            ArbolDePrioridad<LlaveArbolPrioridad> TempArbol = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            if (hospital.Espera.isempty())
            {
                return RedirectToAction("Vacunacion2");
            }
            TempArbol = hospital.Espera.Clone();
            DoubleLinkedList<Paciente> ViewVacunacion = new DoubleLinkedList<Paciente>();
            for (int i = 0; i < hospital.CantidadPasar; i++)
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
            var hospital = F.hospitales.Find(m => Comparador(m, HttpContext.Session.GetString("municipio")) == 0);

            DoubleLinkedList<Paciente> ViewEspera = new DoubleLinkedList<Paciente>();
            ArbolDePrioridad<LlaveArbolPrioridad> PacienteNollego = new ArbolDePrioridad<LlaveArbolPrioridad>(LlaveArbolPrioridad.Compare_Llave_Arbol);
            for (int i = 0; i < hospital.CantidadPasar; i++)
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
            string mydocs = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = "ProyectoF";
            string R = mydocs + '\\' + folder;
            Escribir(R + "\\Tabla.txt", EscribirDatos());
            return View(ViewEspera);
        }
        public IActionResult Buscar()
        {
            if (F.verificar == 0)
            {
                F.verificar = 1;
                Cargardatos();
            }
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
                if (!Temphospital.Espera.isempty())
                {
                    Temparbol = Temphospital.Espera.Clone();
                    while (!Temparbol.isempty())
                    {
                        var templlave = new LlaveArbolPrioridad();
                        templlave = Temparbol.Remove();
                        vista.Add(Temphospital.Pacientes.Find(Convert.ToString(templlave.CodigoHash)));
                    }
                }
            }
            return View(vista);
        }
        public IActionResult Hospital()
        {
            if (F.verificar == 0)
            {
                F.verificar = 1;
                Cargardatos();
            }
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
                HttpContext.Session.SetString("municipio", DatosP.Municipio);
                //F.municipio = DatosP.Municipio;
            }
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            Hospitales hospital = new Hospitales();
            hospital = F.hospitales.Find(m => Comparador(m, HttpContext.Session.GetString("municipio")) == 0);
            if (hospital == null)
            {
                hospital = new Hospitales();
                hospital.Municipio = HttpContext.Session.GetString("municipio");
                F.hospitales.Add(hospital);
            }
            return View();
        }
        public IActionResult ListaVacunados()
        {
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            Hospitales hospital = new Hospitales();
            hospital = F.hospitales.Find(m => Comparador(m, HttpContext.Session.GetString("municipio")) == 0);
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
            if (Tempmaximo.maximo < 1)
            {
                return RedirectToAction("ErrorCantmaximo");
            }
            PacienteComp Comparador = Hospitales.Compare_Municipio;
            Hospitales hospital = new Hospitales();
            hospital = F.hospitales.Find(m => Comparador(m, HttpContext.Session.GetString("municipio")) == 0);
            hospital.CantidadPasar = Tempmaximo.maximo;
            return RedirectToAction("InfoHospital");
        }
        public IActionResult Porcentaje()
        {
            if (F.verificar == 0)
            {
                F.verificar = 1;
                Cargardatos();
            }
            Porcentajes Temp = new Porcentajes();
            Temp.Total = F.total;
            Temp.Vacunados = F.vacunados;
            if (F.total == 0)
            {
                Temp.porcentaje = "0%";
                return View(Temp);
            }
            double porcentaje = Math.Truncate(((Convert.ToDouble(F.vacunados) / Convert.ToDouble(F.total)) * 100));
            Temp.porcentaje = porcentaje + "%";
            return View(Temp);
        }
        public void Escribir(string Archivo, string Texto)
        {

            using (StreamWriter Esc = new StreamWriter(Archivo, false))
            {

                Esc.Write(Texto);
                Esc.Flush();
                Esc.Close();
            }
        }
        public string EscribirDatos()
        {
            string st = "";
            for (int f = 1; f <= F.hospitales.Count2(); f++)
            {
                var Temphospital = new Hospitales();
                Temphospital = F.hospitales.RemoveAt2(f);
                st = st + "L" + "," + Temphospital.Municipio + "," + Temphospital.TempFecha + "," + Temphospital.CantidadPasar + ",";
                for (int i = 0; i < Temphospital.Pacientes.getsize(); i++)
                {
                    for (int h = 1; h <= Temphospital.Pacientes.BuscarCanti(i); h++)
                    {
                        var Temp4 = new Paciente();
                        Temp4 = Temphospital.Pacientes.Remove2(i, h);
                        st = st + Temp4.Id + "," + Temp4.Nombre + "," + Temp4.Apellido + "," + Temp4.DPI + "," + Temp4.Departamento +
                        "," + Temp4.Municipio + "," + Temp4.Edad + "," + Temp4.Areadetrabajo + "," + Temp4.Salud + "," + Temp4.Est + "," + Temp4.Asilo + ","
                        + Temp4.Fecha + "," + Temp4.Paciente_Llego + "," + Temp4.Prioridad + "\n";
                    }
                }
                for (int i = 0; i < Temphospital.PacientesVacunados.getsize(); i++)
                {
                    for (int h = 1; h <= Temphospital.PacientesVacunados.BuscarCanti(i); h++)
                    {
                        var Temp4 = new Paciente();
                        Temp4 = Temphospital.PacientesVacunados.Remove2(i, h);
                        st = st + "T" + "," + Temp4.Id + "," + Temp4.Nombre + "," + Temp4.Apellido + "," + Temp4.DPI + "," + Temp4.Departamento +
                        "," + Temp4.Municipio + "," + Temp4.Edad + "," + Temp4.Areadetrabajo + "," + Temp4.Salud + "," + Temp4.Est + "," + Temp4.Asilo + ","
                        + Temp4.Fecha + "," + Temp4.Paciente_Llego + "," + Temp4.Prioridad + "\n";
                    }
                }


            }
            st = st + "U" + "," + F.vacunados + "," + F.total;
            return st;
        }
        public bool isnull()
        {
            string mydocs = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = "ProyectoF";
            string R = mydocs + '\\' + folder;
            using (TextFieldParser Datos = new TextFieldParser(R + "\\Tabla.txt"))
            {
                Datos.TextFieldType = FieldType.Delimited;
                Datos.SetDelimiters(",");
                string[] Escrito = Datos.ReadFields();
                if (Escrito == null)
                {
                    Datos.Close();
                    return true;
                }
                else
                {
                    Datos.Close();
                    return false;
                }
            }
        }
        public void Cargardatos()
        {
            string mydocs = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = "ProyectoF";
            string R = mydocs + '\\' + folder;
            using (TextFieldParser Datos = new TextFieldParser(R + "\\Tabla.txt"))
            {
                Datos.TextFieldType = FieldType.Delimited;
                Datos.SetDelimiters(",");
                while (!Datos.EndOfData)
                {
                    int espacio = 0;
                    PacienteComp Comparador = Hospitales.Compare_Municipio;

                    Paciente Temp = new Paciente();
                    string[] Escrito = Datos.ReadFields();
                    if (Escrito[0] == "L")
                    {
                        Hospitales Hosp = new Hospitales();
                        Hosp.Municipio = Escrito[1];
                        Hosp.TempFecha = Convert.ToDateTime(Escrito[2]);
                        Hosp.CantidadPasar = Convert.ToInt32(Escrito[3]);
                        F.hospitales.Add(Hosp);

                        espacio = 4;

                    }
                    if (Escrito[0 + espacio] == "T")
                    {
                        Temp.Id = Convert.ToInt32(Escrito[1 + espacio]);
                        Temp.Nombre = Escrito[2 + espacio]; Temp.Apellido = Escrito[3 + espacio]; Temp.DPI = Convert.ToInt64(Escrito[4 + espacio]);
                        Temp.Departamento = Escrito[5 + espacio]; Temp.Municipio = Escrito[6 + espacio];
                        Temp.Edad = Convert.ToInt32(Escrito[7 + espacio]); Temp.Areadetrabajo = Convert.ToInt32(Escrito[8 + espacio]);
                        Temp.Salud = Convert.ToInt32(Escrito[9 + espacio]); Temp.Est = Convert.ToInt32(Escrito[10 + espacio]);
                        Temp.Asilo = Convert.ToInt32(Escrito[11 + espacio]); Temp.Fecha = Convert.ToDateTime(Escrito[12 + espacio]);
                        Temp.Paciente_Llego = Convert.ToBoolean(Escrito[13 + espacio]); Temp.Prioridad = Convert.ToDouble(Escrito[14 + espacio]);
                        var Temphospital = F.hospitales.Find(m => Comparador(m, Temp.Municipio) == 0);
                        var TempTabla = Temphospital.PacientesVacunados;
                        TempTabla.Add(Convert.ToString(Temp.DPI), Temp);
                        var Temparbol = Temphospital.vacunados;
                        LlaveArbolPrioridad Templlave = new LlaveArbolPrioridad();
                        Templlave.CodigoHash = Convert.ToInt64(Temp.DPI);
                        Templlave.Prioridad = Convert.ToDouble(Temp.Prioridad);
                        Temparbol.add(Templlave);
                        F.Tabla_Hash.Add(Convert.ToString(Temp.DPI), Temp);
                        //Arboles de busqueda
                        var TempNombre = new LlaveArbolNombre();
                        var TempDR = new LlaveArbolNumeroDR();
                        var TempApellido = new LlaveArbolApellido();
                        TempDR.CodigoHash = Temp.DPI;
                        TempDR.NumeroDR = Temp.DPI;
                        F.Arbol_NumeroDR.Add(TempDR);

                        TempNombre.Nombre = Temp.Nombre;
                        TempNombre = F.Arbol_Nombre.Find(TempNombre);
                        TempApellido.Apellido = Temp.Apellido;
                        TempApellido = F.Arbol_Apellido.Find(TempApellido);
                        if (TempNombre == null)
                        {
                            TempNombre = new LlaveArbolNombre();
                            TempNombre.CodigoHash = Temp.DPI;
                            TempNombre.Nombre = Temp.Nombre;
                            F.Arbol_Nombre.Add(TempNombre);
                        }
                        if (TempApellido == null)
                        {
                            TempApellido = new LlaveArbolApellido();
                            TempApellido.CodigoHash = Temp.DPI;
                            TempApellido.Apellido = Temp.Apellido;
                            F.Arbol_Apellido.Add(TempApellido);
                        }

                    }
                    else if (Escrito[0 + espacio] == "U")
                    {
                        F.vacunados = Convert.ToInt32(Escrito[1 + espacio]);
                        F.total = Convert.ToInt32(Escrito[2 + espacio]);
                    }
                    else
                    {
                        Temp.Id = Convert.ToInt32(Escrito[0 + espacio]);
                        Temp.Nombre = Escrito[1 + espacio]; Temp.Apellido = Escrito[2 + espacio]; Temp.DPI = Convert.ToInt64(Escrito[3 + espacio]);
                        Temp.Departamento = Escrito[4 + espacio]; Temp.Municipio = Escrito[5 + espacio];
                        Temp.Edad = Convert.ToInt32(Escrito[6 + espacio]); Temp.Areadetrabajo = Convert.ToInt32(Escrito[7 + espacio]);
                        Temp.Salud = Convert.ToInt32(Escrito[8 + espacio]); Temp.Est = Convert.ToInt32(Escrito[9 + espacio]);
                        Temp.Asilo = Convert.ToInt32(Escrito[10 + espacio]); Temp.Fecha = Convert.ToDateTime(Escrito[11 + espacio]);
                        Temp.Paciente_Llego = Convert.ToBoolean(Escrito[12 + espacio]); Temp.Prioridad = Convert.ToDouble(Escrito[13 + espacio]);
                        var Temphospital = F.hospitales.Find(m => Comparador(m, Temp.Municipio) == 0);
                        var TempTabla = Temphospital.Pacientes;
                        TempTabla.Add(Convert.ToString(Temp.DPI), Temp);
                        LlaveArbolPrioridad Templlave = new LlaveArbolPrioridad();
                        Templlave.CodigoHash = Convert.ToInt64(Temp.DPI);
                        Templlave.Prioridad = Convert.ToDouble(Temp.Prioridad);
                        if (Temp.Fecha.Year == 1)
                        {
                            var temparbol = Temphospital.Registrar;
                            temparbol.add(Templlave);
                        }
                        else
                        {
                            var temparbol = Temphospital.Espera;
                            temparbol.add(Templlave);
                        }
                        F.Tabla_Hash.Add(Convert.ToString(Temp.DPI), Temp);
                        //Arboles de busqueda
                        var TempNombre = new LlaveArbolNombre();
                        var TempDR = new LlaveArbolNumeroDR();
                        var TempApellido = new LlaveArbolApellido();
                        TempDR.CodigoHash = Temp.DPI;
                        TempDR.NumeroDR = Temp.DPI;
                        F.Arbol_NumeroDR.Add(TempDR);

                        TempNombre.Nombre = Temp.Nombre;
                        TempNombre = F.Arbol_Nombre.Find(TempNombre);
                        TempApellido.Apellido = Temp.Apellido;
                        TempApellido = F.Arbol_Apellido.Find(TempApellido);
                        if (TempNombre == null)
                        {
                            TempNombre = new LlaveArbolNombre();
                            TempNombre.CodigoHash = Temp.DPI;
                            TempNombre.Nombre = Temp.Nombre;
                            F.Arbol_Nombre.Add(TempNombre);
                        }
                        if (TempApellido == null)
                        {
                            TempApellido = new LlaveArbolApellido();
                            TempApellido.CodigoHash = Temp.DPI;
                            TempApellido.Apellido = Temp.Apellido;
                            F.Arbol_Apellido.Add(TempApellido);
                        }
                    }
                    

                }
                Datos.Close();
            }
        }
        public IActionResult ErrorRegVac()
        {
            return View();
        }
        public IActionResult ErrorCantmaximo()
        {
            return View();
        }
    }

}

