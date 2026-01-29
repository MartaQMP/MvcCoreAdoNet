using Microsoft.AspNetCore.Mvc;
using MvcCoreAdoNet.Models;
using MvcCoreAdoNet.Repositories;

namespace MvcCoreAdoNet.Controllers
{
    public class HospitalController : Controller
    {
        RepositoryHospital repo;

        public HospitalController()
        {
            this.repo = new RepositoryHospital();
        }
        public async Task<IActionResult> Index()
        {
            List<Hospital> hospitales = await this.repo.GetHospitalesAsync();
            return View(hospitales);
        }

        /* DETALLES */
        public async Task<IActionResult> Detalles(int idHospital)
        {
            Hospital hospital = await this.repo.FindHospitalAsync(idHospital);
            return View(hospital);
        }

        /* GET POST CREAR HOSPITAL */
        public IActionResult Create()
        {
            return View();
        }

        //En los métodos POST de los controladores SI que recibimos los objetos 
        [HttpPost]
        public async Task<IActionResult> Create(Hospital hospital)
        {
            await this.repo.InsertHospitalAsync (hospital.IdHospital, hospital.Nombre, hospital.Direccion, hospital.Telefono, hospital.Camas);
            ViewData["MENSAJE"] = "Hospital insertado";
            return View();
        }

        /* GET POST MODIFICAR */
        public async Task<IActionResult> Edit(int id)
        { 
            Hospital hospital = await this.repo.FindHospitalAsync(id);
            return View(hospital);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Hospital hospital)
        {
            await this.repo.UpdateHospitalAsync (hospital.IdHospital, hospital.Nombre, hospital.Direccion, hospital.Telefono, hospital.Camas);
            ViewData["MENSAJE"] = "Hospital modificado";
            return View();
        }

        /* DELETE */
        public async Task<IActionResult> Delete(int id)
        {
            await this.repo.DeleteHospitalAsync(id);
            return RedirectToAction("Index");
        }
    }
}
