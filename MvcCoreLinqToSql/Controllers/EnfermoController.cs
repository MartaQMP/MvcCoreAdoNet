using Microsoft.AspNetCore.Mvc;
using MvcCoreLinqToSql.Models;
using MvcCoreLinqToSql.Repositories;

namespace MvcCoreLinqToSql.Controllers
{
    public class EnfermoController : Controller
    {
        RepositoryEnfermos repo;

        public EnfermoController()
        {
            this.repo = new RepositoryEnfermos();
        }

        public IActionResult Index()
        {
            List<Enfermo> enfermos = this.repo.GetEnfermos();
            if(enfermos == null)
            {
                ViewBag.Mensaje = "No hay enfermos";
                return View();
            }
            return View(enfermos);
        }
        public IActionResult Details(string inscripcion)
        {
            Enfermo enfermo = this.repo.GetEnfermoDetails(inscripcion);
            if(enfermo == null)
            {
                ViewBag.Mensaje = "No hay datos del enfermo";
                return View();
            }
            return View(enfermo);
        }

        public IActionResult Delete(string inscripcion)
        {
            int registro = this.repo.DeleteEnfermo(inscripcion);
            return RedirectToAction("Index");
        }
    }
}
