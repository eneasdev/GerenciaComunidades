using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Novo.Infra;

namespace Novo.Controllers
{
    public class AmbienteController : Controller
    {
        private readonly Contexto _context;

        public AmbienteController(Contexto context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Ambiente()
        {
            var ambientes = _context.Ambientes.ToList();
            return View(ambientes);
        }
    }
}
