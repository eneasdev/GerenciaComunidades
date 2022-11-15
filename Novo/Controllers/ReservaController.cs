using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Novo.Infra;
using Novo.Models.Domain;
using Novo.Models.Enums;
using Novo.Models.ReservaModels;
using Novo.Services;

namespace Novo.Controllers
{
    public class ReservaController : Controller
    {
        private readonly GeComuContext _context;
        private readonly IReservaService _reservaService;
        private readonly UserManager<Usuario> _userManager;

        public ReservaController(GeComuContext context, IReservaService reservaService, UserManager<Usuario> userManager)
        {
            _context = context;
            _reservaService = reservaService;
            _reservaService.ResetarReservas();
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult ListarAmbientes(string periodo, string dia)
        {
            var ambientesList = new List<ListarAmbientesViewModel>();
            var reservasList = new List<Reserva>();

            DateTime dataInicial = default;
            DateTime dataFinal = default;

            IQueryable<ListarAmbientesViewModel> query;

            if (!string.IsNullOrWhiteSpace(periodo) && !string.IsNullOrWhiteSpace(dia))
            {
                var periodoInicial = int.Parse(periodo[..2]);
                var periodoFinal = int.Parse(periodo[2..]);

                dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, int.Parse(dia), periodoInicial, 00, 00);
                dataFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, int.Parse(dia), periodoFinal, 00, 00);

                query = from a in _context.Ambientes
                        where a.Status != Status.Desativado && a.Status != Status.Outro
                        select new ListarAmbientesViewModel()
                        {
                            Descricao = a.Descricao,
                            IdAmbiente = a.IdAmbiente,
                            QtdItens = a.Items.Count()
                        };

                var queryRsv = from r in _context.Reservas
                               where r.DataInicial >= dataInicial && r.DataFinal <= dataFinal
                               where r.Status == StatusReserva.Reservado
                               select r;

                ambientesList.AddRange(query.Distinct());
                reservasList.AddRange(queryRsv.Distinct());

                foreach (var ambiente in ambientesList)
                {
                    if (reservasList.Count > 0)
                    {
                        foreach (var reserva in reservasList)
                        {
                            if (reserva.IdAmbiente == ambiente.IdAmbiente)
                            {
                                ambiente.StatusReserva = StatusReserva.Reservado;
                            }
                            else
                            {
                                ambiente.StatusReserva = StatusReserva.Livre;
                            }
                        }
                    }
                    else
                    {

                        ambiente.StatusReserva = StatusReserva.Livre;
                    }

                }

            }
            else
            {
                query = from a in _context.Ambientes
                        where a.Status != Status.Desativado && a.Status != Status.Outro
                        select new ListarAmbientesViewModel()
                        {
                            Descricao = a.Descricao,
                            StatusReserva = StatusReserva.Livre,
                            IdAmbiente = a.IdAmbiente,
                            QtdItens = a.Items.Count()
                        };

                ambientesList.AddRange(query);

                foreach (var ambiente in ambientesList)
                {
                    ambiente.StatusReserva = StatusReserva.Livre;
                }
            }

            var diasMesAtual = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            var diasMes = new List<SelectListItem>();

            for (var i = 1; i < diasMesAtual; i++)
            {
                diasMes.Add(new SelectListItem(i.ToString(), i.ToString()));
            }

            ViewBag.Dia = diasMes;

            return View(ambientesList);
        }

        [HttpGet]
        public IActionResult ReservarAmbiente(int id)
        {
            var name = User.Identity.Name;

            var user = _userManager.FindByNameAsync(name).Result;

            var ambienteBd = _context.Ambientes.FirstOrDefault(x => x.IdAmbiente == id);

            var ambiente = new ReservarAmbienteViewModel
            {
                Descricao = ambienteBd.Descricao,
                Status = ambienteBd.Status,
                IdAmbiente = id,
                IdUsuario = user.Id
            };

            var diasMesAtual = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            var diasMes = new List<SelectListItem>();

            for (var i = 1; i < diasMesAtual; i++)
            {
                diasMes.Add(new SelectListItem(i.ToString(), i.ToString()));
            }

            ViewBag.Dia = diasMes;

            var userRole = _userManager.GetRolesAsync(user).Result;

            if (userRole.Contains("Administrador"))
            {
                var usuarios = new List<SelectListItem>();

                var queryy = from usuario in _context.Usuarios
                             join userRoles in _context.UserRoles on usuario.Id equals userRoles.UserId
                             join roles in _context.Roles on userRoles.RoleId equals roles.Id
                             where roles.Id != "1" && roles.Id != "2"
                             select new SelectListItem
                             {
                                 Value = usuario.Id,
                                 Text = usuario.Email
                             };

                usuarios.AddRange(queryy);

                ViewBag.Usuario = usuarios;
            }

            return View(ambiente);
        }

        [HttpPost]
        public IActionResult ReservarAmbiente(ReservarAmbienteViewModel model)
        {
            var ambiente = _context.Ambientes.FirstOrDefault(x => x.IdAmbiente == model.IdAmbiente);

            if (ambiente is null) return NotFound();

            DateTime dataInicial = default;
            DateTime dataFinal = default;

            IQueryable<ListarAmbientesViewModel> query;

            if (string.IsNullOrWhiteSpace(model.Periodo) && string.IsNullOrWhiteSpace(model.Dia))
            {
                TempData["ErrorMessage"] = "Periodo e dia devem ser informados.";
                return BadRequest();
            }
            var periodoInicial = int.Parse(model.Periodo[..2]);
            var periodoFinal = int.Parse(model.Periodo[2..]);

            dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, int.Parse(model.Dia), periodoInicial, 00, 00);
            dataFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, int.Parse(model.Dia), periodoFinal, 00, 00);

            var reservaExiste = _reservaService.ReservaExiste(model.IdAmbiente, dataInicial, dataFinal);

            if (reservaExiste)
            {
                TempData["ErrorMessage"] = "Reserva Inválida.";
                return BadRequest();
            }

            var name = User.Identity.Name;

            var user = _userManager.FindByNameAsync(name).Result;

            var userRole = _userManager.GetRolesAsync(user).Result;

            if (userRole.Contains("Administrador"))
            {
                var novaReserva = new Reserva(
                    dataInicial: dataInicial,
                    dataFinal: dataFinal,
                    idAmbiente: model.IdAmbiente,
                    idUsuario: model.Usuario
                );

                _context.Reservas.AddAsync(novaReserva);
                _context.SaveChanges();
            }
            else
            {
                var novaReserva = new Reserva(
                    dataInicial: dataInicial,
                    dataFinal: dataFinal,
                    idAmbiente: model.IdAmbiente,
                    idUsuario: model.IdUsuario
                );

                _context.Reservas.AddAsync(novaReserva);
                _context.SaveChanges();
            }

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

            if (reserva == null) return NotFound();

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

        //[HttpGet]
        //public IActionResult ListarItems(ListarItemsViewModel viewModel)
        //{
        //    var ambientes = _context.Ambientes.Where(x => x.Status == Status.Ativo).ToList();

        //    var model = new ListarItemsViewModel();

        //    model.Ambientes = new SelectList(ambientes, "Descricao", "Descricao");

        //    return View(model);
        //}

        //[HttpGet]
        //public IActionResult ReservarItem(int id)
        //{
        //    var item = _context.Acentos.FirstOrDefault(x => x.IdItem == id);

        //    return View(item);
        //}

        //[HttpPost]
        //public IActionResult ReservarItem(ReservarItemViewModel reservarItemModel)
        //{
        //    var item = _context.Acentos.FirstOrDefault(x => x.IdItem == reservarItemModel.IdItem);

        //    if (item is null) return NotFound();

        //    var novaReserva = new Reserva(
        //            dataInicial: reservarItemModel.DataInicial,
        //            dataFinal: reservarItemModel.DataFinal,
        //            idItem: reservarItemModel.IdItem,
        //            idUsuario: reservarItemModel.IdUsuario
        //        );

        //    _context.Reservas.AddAsync(novaReserva);
        //    _context.SaveChanges();

        //    return RedirectToAction("CriarReserva");
        //}

        //[HttpGet]
        //public IActionResult AtualizarReservaItem(int id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult AtualizarReservaItem()
        //{
        //    return View();
        //}
    }
}
