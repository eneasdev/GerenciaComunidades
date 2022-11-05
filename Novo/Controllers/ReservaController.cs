using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Novo.Infra;
using Novo.Models.Domain;
using Novo.Models.Enums;
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
            _reservaService.ResetarReservas();
        }

        [HttpGet]
        public IActionResult Reservar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListarAmbientes()
        {
            var reservas = _context.Reservas
                .Include(x => x.Ambiente)
                .Where(x => x.IdAmbiente == x.Ambiente.IdAmbiente).ToList();

            var listaAmbientes = reservas.Select(x => new ListarAmbientesViewModel
            {
                IdAmbiente = x.IdAmbiente.GetValueOrDefault(),
                Descricao = x.Ambiente.Descricao,
                Status = x.Status,
                QtdItens = x.Ambiente.Items.Count()
            }).ToList();

            var algo = reservas.Distinct();

            return View(listaAmbientes);
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

            if (ambiente is null) return NotFound();


            var reservaExiste = _reservaService.ReservaExiste(reservarAmbienteModel);

            if (reservaExiste)
            {
                TempData["ErrorMessage"] = "Reserva Inválida";
                return BadRequest();
            }

            var novaReserva = new Reserva(
                    dataInicial: reservarAmbienteModel.DataInicial,
                    dataFinal: reservarAmbienteModel.DataFinal,
                    idAmbiente: reservarAmbienteModel.IdAmbiente,
                    idUsuario: reservarAmbienteModel.IdUsuario
                );

            ambiente.Reservar();

            _context.Reservas.AddAsync(novaReserva);
            _context.SaveChanges();

            TempData["Message"] = "Reservado com sucesso";
            return RedirectToAction("ListarAmbientes");
        }

        [HttpGet]
        public IActionResult AtualizarReservaAmbiente(int id)
        {
            return View(_context.Reservas.FirstOrDefault(x => x.IdAmbiente == id));
        }

        [HttpPost]
        public IActionResult AtualizarReservaAmbiente(AtualizarReservaAmbienteViewModel model)
        {
            var reserva = _context.Reservas.FirstOrDefault(x => x.IdReserva == model.IdReserva);

            if(reserva == null) return NotFound();

            reserva.Atualizar(model.DataInicial, model.DataFinal);

            return RedirectToAction("CriarReserva");
        }

        //[HttpGet]
        //public IActionResult ListarItems()
        //{
        //    var ambientes = _context.Ambientes.Where(x => x.Status == Status.Reservado).Include(x => x.Items).ToList();

        //    var viewModel = new ListarItemsViewModel();
        //    viewModel.Ambientes = ambientes.ToList();

        //    return View(viewModel);
        //}

        [HttpGet]
        public IActionResult ListarItems(ListarItemsViewModel viewModel)
        {
            var ambientes = _context.Ambientes.Where(x => x.Status == Status.Reservado).ToList();

            var model = new ListarItemsViewModel();

            model.Ambientes = new SelectList(ambientes, "Descricao", "Descricao");

            return View(model);
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

            if (item is null) return NotFound();

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

        [HttpGet]
        public IActionResult AtualizarReservaItem(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult AtualizarReservaItem()
        {
            return View();
        }
    }
}
