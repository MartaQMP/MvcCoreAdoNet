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
    }
}
