using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Novo.Infra;
using Novo.Models;
using System.Diagnostics;

namespace Novo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Contexto _context;

        public HomeController(ILogger<HomeController> logger, Contexto context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string login, string senha)
        {
            var usuario = new Usuario(login, senha);

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            var existe = _context.Usuarios.Any(u => u.Login == login);

            return View();
        }

        public IActionResult Login(string Login, string Senha)
        {
            return View();
        }
    }
}