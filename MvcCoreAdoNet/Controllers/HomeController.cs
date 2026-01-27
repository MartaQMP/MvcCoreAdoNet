using Microsoft.AspNetCore.Mvc;

namespace MvcCoreAdoNet.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Plantilla2()
        {
            return View();
        }
    }
}
