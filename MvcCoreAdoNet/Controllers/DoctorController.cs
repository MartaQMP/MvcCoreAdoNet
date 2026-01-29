using Microsoft.AspNetCore.Mvc;
using MvcCoreAdoNet.Models;
using MvcCoreAdoNet.Repositories;
using System.Threading.Tasks;

namespace MvcCoreAdoNet.Controllers
{
    public class DoctorController : Controller
    {
        RepositoryHospital repo;
        public DoctorController()
        {
            this.repo = new RepositoryHospital();
        }
        public async Task<IActionResult> DoctorEspecialidad()
        {
            List<Doctor> doctores = await this.repo.GetDoctoresAsync();
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            ViewData["ESPECIALIDADES"] = especialidades;
            return View(doctores);
        }

        [HttpPost]
        public async Task<IActionResult> DoctorEspecialidad(string especialidad)
        {
            List<Doctor> doctores = await this.repo.GetDoctoresEspecialidadAsync(especialidad);
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            ViewData["ESPECIALIDADES"] = especialidades;
            return View(doctores);
        }



    }
}
