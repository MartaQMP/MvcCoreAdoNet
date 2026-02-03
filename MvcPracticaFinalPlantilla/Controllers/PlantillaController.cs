using Microsoft.AspNetCore.Mvc;
using MvcPracticaFinalPlantilla.Models;
using MvcPracticaFinalPlantilla.Repositories;

namespace MvcPracticaFinalPlantilla.Controllers
{
    public class PlantillaController : Controller
    {
        RepositoryPlantilla repo;

        public PlantillaController()
        {
            this.repo = new RepositoryPlantilla();
        }

        public IActionResult Index()
        {
            List<Plantilla> plantillas = this.repo.GetPlantilla();
            if(plantillas == null)
            {
                ViewBag.Mensaje = "No se ha encontrado la plantilla";
                return View();
            }
            return View(plantillas);
        }

        public async Task<IActionResult> Delete(int empleado_no)
        {
            await this.repo.DeletePlantillaAsync(empleado_no);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int empleado_no)
        {
            Plantilla plantilla = this.repo.GetEmpleadoPlantillaById(empleado_no);
            return View(plantilla);
        }

        public IActionResult BuscadorPlantilla()
        {
            List<string> funciones = this.repo.GetFunciones();
            ViewBag.Funciones = funciones;
            return View();
        }


        [HttpPost]
        public IActionResult BuscadorPlantilla(string funcion)
        {
            List<string> funciones = this.repo.GetFunciones();
            ViewBag.Funciones = funciones;
            ResumenPlantilla resumen = this.repo.GetPlantillaByFuncion(funcion);
            return View(resumen);
        }

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(int hospital_cod, int sala_cod, int empleado_no, string apellido, string funcion, int salario)
        {
            Plantilla plantilla = new Plantilla
            {
                HospitalCod = hospital_cod,
                SalaCod = sala_cod,
                EmpleadoNo = empleado_no,
                Apellido = apellido,
                Funcion = funcion,
                Salario = salario
            };
            await this.repo.UpsertPlantillaAsync(plantilla);
            return RedirectToAction("Index");
        }

        public IActionResult Update(int empleado_no)
        {
            Plantilla plantilla = this.repo.GetEmpleadoPlantillaById(empleado_no);
            return View(plantilla);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int hospital_cod, int sala_cod, int empleado_no, string apellido, string funcion, int salario)
        {
            Plantilla plantilla = new Plantilla
            {
                HospitalCod = hospital_cod,
                SalaCod = sala_cod,
                EmpleadoNo = empleado_no,
                Apellido = apellido,
                Funcion = funcion,
                Salario = salario
            };
            await this.repo.UpsertPlantillaAsync(plantilla);
            return RedirectToAction("Index");
        }
    }
}
