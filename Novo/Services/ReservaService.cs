using Novo.Infra;

namespace Novo.Services
{
    public class ReservaService : IReservaService
    {
        private readonly Contexto _context;

        public ReservaService(Contexto context)
        {
            _context = context;
        }

        public void ValidarStatusReservas()
        {
            var ambientes = _context.Ambientes.ToList();

            var reservas = _context.Reservas.ToList();

            foreach (var reserva in reservas)
            {
                var ambiente = ambientes.FirstOrDefault(x => x.IdAmbiente == reserva.IdAmbiente);

                if (reserva.DataFinal <= DateTime.Now)
                {
                    ambiente.Status = Models.Enums.Status.Livre;
                }

                _context.Ambientes.Update(ambiente);
            }

            _context.SaveChanges();
        }
    }
}
