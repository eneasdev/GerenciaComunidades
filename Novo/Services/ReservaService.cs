using Novo.Infra;
using Novo.Models.Enums;

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
            var reservas = _context.Reservas.Where(x => x.Status == StatusReserva.Reservado &&
                                                        x.DataFinal <= DateTime.Now).ToList();

            foreach (var reserva in reservas)
            {
                reserva.Status = StatusReserva.Reservado;
                _context.Reservas.Update(reserva);
            }

            _context.SaveChanges();
        }

        public bool ReservaExiste(int idAmbiente, DateTime dataInicial, DateTime dataFinal)
        {
            var reserva = _context.Reservas
                .FirstOrDefault(x =>
                x.IdAmbiente == idAmbiente &&
                x.DataInicial <= dataInicial &&
                x.DataFinal >= dataFinal);

            if (reserva is null)
            {
                return false;
            }
            return true;
        }
    }
}
