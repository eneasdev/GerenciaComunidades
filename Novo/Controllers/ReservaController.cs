using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Novo.Infra;
using Novo.Models.Domain;
using Novo.Models.ReservaModels;
using Novo.Services;

namespace Novo.Controllers
{
    [Authorize]
    public class ReservaController : Controller
    {
        private readonly Contexto _context;
        private readonly IReservaService _reservaService;

        public ReservaController(Contexto context, IReservaService reservaService)
        {
            _context = context;
            _reservaService = reservaService;
            _reservaService.ValidarStatusReservas();
        }

        [HttpGet]
        public IActionResult CriarReserva()
        {
            var ambientes = _context.Ambientes.ToList();

            return View(ambientes);
        }

        [HttpGet]
        public IActionResult Reservar(int id)
        {
            var nome = HttpContext.User.Identity.Name;
            var user = _context.Usuarios.FirstOrDefault(x => x.Login == nome);

            var ambienteBd = _context.Ambientes.FirstOrDefault(x => x.IdAmbiente == id);

            var ambiente = new ReservaUsuarioModel
            {
                Descricao = ambienteBd.Descricao,
                Status = ambienteBd.Status,
                IdAmbiente = id,
                IdUsuario = user.IdUsuario
            };

            return View(ambiente);
        }

        [HttpPost]
        public IActionResult Reservar(ReservaUsuarioModel reservaUsuarioModel)
        {
            var ambiente = _context.Ambientes.FirstOrDefault(x => x.IdAmbiente == reservaUsuarioModel.IdAmbiente);

            var novaReserva = new Reserva(
                dataInicial: reservaUsuarioModel.DataInicial,
                dataFinal: reservaUsuarioModel.DataFinal,
                idAmbiente: reservaUsuarioModel.IdAmbiente,
                idUsuario: reservaUsuarioModel.IdUsuario
                );

            ambiente.Reservar();

            _context.Reservas.AddAsync(novaReserva);
            _context.SaveChanges();

            return RedirectToAction("CriarReserva");
        }
    }
}
