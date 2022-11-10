using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Novo.Infra;
using Novo.Models.AmbienteModels;
using Novo.Models.Domain;
using Novo.Models.Enums;

namespace Novo.Controllers
{
    public class AmbienteController : Controller
    {
        private readonly Contexto _context;

        public AmbienteController(Contexto context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Ambientes()
        {
            var ambientes = _context.Ambientes.Include(a => a.Items)
                .Where(x => x.Status != Status.Desativado)
                .ToList();

            return View(ambientes);
        }

        [HttpGet]
        public IActionResult CriarAmbiente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CriarAmbiente(CriarAmbienteViewModel model)
        {
            var novoAmbiente = new Ambiente(model.Descricao);
            novoAmbiente.CriarAcentos(model.QtdItems);

            _context.Ambientes.Add(novoAmbiente);
            _context.SaveChanges();
            TempData["Message"] = "Ambiente criado com sucesso!";

            return RedirectToAction("Ambientes");
        }

        [HttpGet]
        public IActionResult AtualizarAmbiente(int id)
        {
            var ambiente = _context.Ambientes.FirstOrDefault(x => x.IdAmbiente == id);
            if (ambiente == null) return View();
            var ambienteView = new AtualizarAmbienteViewModel()
            {
                Descricao = ambiente.Descricao,
                IdAmbiente = ambiente.IdAmbiente,
                Items = ambiente.Items.ToList()
            };

            return View(ambienteView);
        }

        [HttpPost]
        public IActionResult AtualizarAmbiente(AtualizarAmbienteViewModel model)
        {
            var ambiente = _context.Ambientes.FirstOrDefault(x => x.IdAmbiente == model.IdAmbiente);

            if (ambiente is null) return View();

            if (!string.IsNullOrWhiteSpace(model.Descricao))
            {
                ambiente.Descricao = model.Descricao;
            }

            if (model.QtdNovosItems > 0)
            {
                ambiente.CriarAcentos(model.QtdNovosItems);
            }

            if (model.ListaItemsExcluidos != null)
            {
                var items = _context.Acentos.Where(x => model.ListaItemsExcluidos.Contains(x.IdItem)).ToList();
                _context.Acentos.RemoveRange(items);
            }

            _context.Ambientes.Update(ambiente);
            _context.SaveChanges();

            return View();
        }

        [HttpPost]
        public IActionResult DesativarAmbiente(int id)
        {
            var ambiente = _context.Ambientes.FirstOrDefault(x => x.IdAmbiente == id);

            if (ambiente is null) return NotFound("Não encontrado.");

            ambiente.Desativar();
            _context.Ambientes.Update(ambiente);
            _context.SaveChanges();

            return RedirectToAction("Ambientes");
        }
    }
}
