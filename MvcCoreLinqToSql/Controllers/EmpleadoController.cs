using Microsoft.AspNetCore.Mvc;
using MvcCoreLinqToSql.Models;
using MvcCoreLinqToSql.Repositories;

namespace MvcCoreLinqToSql.Controllers
{
    public class EmpleadoController : Controller
    {
        RepositoryEmpleados repo;

        public EmpleadoController()
        {
            this.repo = new RepositoryEmpleados();
        }

        public IActionResult Index()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();
            return View(empleados);
        }

        public IActionResult Details( int id)
        {
            Empleado emp = this.repo.GetEmpleadoById(id);
            return View(emp);
        }

        public IActionResult BuscadorEmpleados()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BuscadorEmpleados(string oficio, int salario)
        {
            List<Empleado> empleados = this.repo.GetEmpleadosOficioSalario(oficio, salario);
            if(empleados == null)
            {
                ViewBag.Mensaje = "No existen empleados con ese oficio y un salario mayor a " + salario;
                return View();
            }
            return View(empleados);
        }

        public IActionResult DatosEmpleados()
        {
            List<string> oficios = this.repo.GetOficios();
            ViewBag.Oficios = oficios;
            return View();
        }

        [HttpPost]
        public IActionResult DatosEmpleados(string oficio)
        {
            List<string> oficios = this.repo.GetOficios();
            ViewBag.Oficios = oficios;
            ResumenEmpleados resumen = this.repo.GetResumenEmpleadosOficio(oficio);
            return View(resumen);
        }
    }
}
