using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Novo.Infra;
using Novo.Models.Enums;

namespace Novo.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeComuContext _context;

        public HomeController(GeComuContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuario = "Anônimo";
            var autenticado = false;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                usuario = HttpContext.User.Identity.Name;
                autenticado = true;
            }
            else
            {
                usuario = "";
                autenticado = false;
            }

            ViewBag.usuario = usuario;
            ViewBag.autenticado = autenticado;
            return View();
        }

        //[Authorize]
        //public IActionResult CriarUsuario(string login, string senha)
        //{

        //    var usuario = new Usuario(login, senha);

        //    _context.Usuarios.Add(usuario);
        //    _context.SaveChanges();

        //    return View();
        //}
    }
}