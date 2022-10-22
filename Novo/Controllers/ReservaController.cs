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
        public IActionResult ReservarAmbiente(int id)
        {
            var nome = HttpContext.User.Identity.Name;
            var user = _context.Usuarios.FirstOrDefault(x => x.Login == nome);

            var ambienteBd = _context.Ambientes.FirstOrDefault(x => x.IdAmbiente == id);

            var ambiente = new ReservarAmbienteViewModel
            {
                Descricao = ambienteBd.Descricao,
                Status = ambienteBd.Status,
                IdAmbiente = id,
                IdUsuario = user.IdUsuario
            };

            return View(ambiente);
        }

        [HttpPost]
        public IActionResult ReservarAmbiente(ReservarAmbienteViewModel reservarAmbienteModel)
        {
            var ambiente = _context.Ambientes.FirstOrDefault(x => x.IdAmbiente == reservarAmbienteModel.IdAmbiente);

            var novaReserva = new Reserva(
                dataInicial: reservarAmbienteModel.DataInicial,
                dataFinal: reservarAmbienteModel.DataFinal,
                idAmbiente: reservarAmbienteModel.IdAmbiente,
                idUsuario: reservarAmbienteModel.IdUsuario
                );

            ambiente.Reservar();

            _context.Reservas.AddAsync(novaReserva);
            _context.SaveChanges();

            return RedirectToAction("CriarReserva");
        }

        [HttpGet]
        public IActionResult ReservarItem(int id)
        {
            var item = _context.Acentos.FirstOrDefault(x => x.IdItem == id);

            return View(item);
        }

        [HttpPost]
        public IActionResult ReservarItem(ReservarItemViewModel reservarItemModel)
        {
            var item = _context.Acentos.FirstOrDefault(x => x.IdItem == reservarItemModel.IdItem);

            var novaReserva = new Reserva(
                dataInicial: reservarItemModel.DataInicial,
                dataFinal: reservarItemModel.DataFinal,
                idItem: reservarItemModel.IdItem,
                idUsuario: reservarItemModel.IdUsuario
                );

            item.Reservar();

            _context.Reservas.AddAsync(novaReserva);
            _context.SaveChanges();

            return RedirectToAction("CriarReserva");
        }
    }
}
