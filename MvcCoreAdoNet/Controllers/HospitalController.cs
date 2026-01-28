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
        public async Task<IActionResult> Detalles(int idHospital)
        {
            Hospital hospital = await this.repo.FindHospitalAsync(idHospital);
            return View(hospital);
        }
    }
}
