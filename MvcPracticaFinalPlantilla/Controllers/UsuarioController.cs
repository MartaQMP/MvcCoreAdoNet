using Microsoft.AspNetCore.Mvc;
using MvcPracticaFinalPlantilla.Models;
using MvcPracticaFinalPlantilla.Repositories;
using System.Threading.Tasks;

namespace MvcPracticaFinalPlantilla.Controllers
{
    public class UsuarioController : Controller
    {
        RepositoryUsuario repo;

        public UsuarioController() 
        { 
            this.repo = new RepositoryUsuario(); 
        
        }
        public IActionResult Index()
        {
            List<Usuario> usuarios = this.repo.GetUsuarios();
            if(usuarios == null)
            {
                ViewBag.Mensaje = "No se encuentran usuarios";
                return View();
            }
            return View(usuarios);
        }

        public async Task<IActionResult> Informacion(int idUsuario)
        {
            InformacionUsuario info = await this.repo.GetInformacionUsuario(idUsuario);
            return View(info);
        }

    }
}
