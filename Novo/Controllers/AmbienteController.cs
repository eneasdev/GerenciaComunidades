using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Novo.Infra;
using Novo.Models.AmbienteModels;
using Novo.Models.Domain;

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
        public IActionResult CriarAmbiente()
        {
            var ambientes = _context.Ambientes.ToList();
            return View(ambientes);
        }

        [HttpPost]
        public IActionResult CriarAmbiente(CriarAmbienteViewModel model)
        {
            var novoAmbiente = new Ambiente(model.Descricao);
            novoAmbiente.CriarAcentos(model.QtdItems);

            _context.Ambientes.Add(novoAmbiente);
            _context.SaveChanges();

            return View();
        }

        [HttpGet]
        public IActionResult AtualizarAmbiente(int id)
        {
            var ambiente = _context.Ambientes.FirstOrDefault(x => x.IdAmbiente == id);
            return View(ambiente);
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
    }
}
