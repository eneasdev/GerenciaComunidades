using Novo.Infra;
using Novo.Models.ReservaModels;
using System.Reflection.Metadata.Ecma335;

namespace Novo.Services
{
    public class ReservaService : IReservaService
    {
        private readonly Contexto _context;

        public ReservaService(Contexto context)
        {
            _context = context;
        }

        public void ResetarReservas()
        {
            var ambientes = _context.Ambientes.ToList();

            var items = _context.Acentos.ToList();

            var reservas = _context.Reservas.ToList();

            foreach (var reserva in reservas)
            {
                if (reserva.IdAmbiente != null)
                {
                    var ambiente = ambientes.FirstOrDefault(ambienteAtual => ambienteAtual.IdAmbiente == reserva.IdAmbiente);

                    if (ambiente != null && reserva.DataFinal <= DateTime.Now)
                    {
                        ambiente.Status = Models.Enums.Status.Livre;
                        _context.Ambientes.Update(ambiente);
                    }
                }
                
                if (reserva.IdItem != null)
                {
                    var item = items.FirstOrDefault(x => x.IdItem == reserva.IdItem);

                    if (item != null && reserva.DataFinal <= DateTime.Now)
                    {
                        item.Status = Models.Enums.Status.Livre;
                        _context.Acentos.Update(item);
                    }
                }
            }

            _context.SaveChanges();
        }

        public bool ReservaExiste(ReservarAmbienteViewModel candidata)
        {
            var reserva = _context.Reservas
                .FirstOrDefault(x => 
                x.IdAmbiente == candidata.IdAmbiente &&
                x.DataInicial <= candidata.DataInicial &&
                x.DataFinal >= candidata.DataFinal);

            if (reserva is null)
            {
                return false;
            }
            return true;
        }
    }
}
