using Microsoft.AspNetCore.Mvc;
using MvcCoreCrudDepartamentosAdo.Models;
using MvcCoreCrudDepartamentosAdo.Repositories;
using System.Threading.Tasks;

namespace MvcCoreCrudDepartamentosAdo.Controllers
{
    public class DepartamentoController : Controller
    {
        RepositoryDepartamento repo;

        public DepartamentoController()
        {
            this.repo = new RepositoryDepartamento();
        }
        public async Task<IActionResult> Index()
        {
            List<Departamento> departamentos = await this.repo.GetDepartamentosAsync();
            return View(departamentos);
        }

        /* GET POST CREATE */
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Departamento dept)
        {
            await this.repo.InsertDepartamentoAsync(dept.IdDepartamento, dept.Nombre, dept.Localidad);
            return RedirectToAction("Index");
        }

        /* GET POST EDIT */
        public async Task<IActionResult> Edit(int id)
        {
            Departamento dept = await this.repo.FindDepartamentoAsync(id);
            return View(dept);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Departamento dept)
        {
            await this.repo.UpdateDepartamentoAsync(dept.IdDepartamento, dept.Nombre, dept.Localidad);
            return RedirectToAction("Index");
        }

        /* DELETE */
        public async Task<IActionResult> Delete(int id)
        {
            await this.repo.DeleteDepartamentoAsync(id);
            return RedirectToAction("Index");
        }
    }
}
