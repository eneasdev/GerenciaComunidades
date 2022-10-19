using Novo.Models.Enums;

namespace Novo.Models
{
    public class Reserva
    {
        public Reserva(DateTime dataInicial, DateTime dataFinal, Status status)
        {
            Status = status;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
        }

        public int IdReserva { get; set; }
        public Status Status { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }

        public int IdAmbiente { get; set; }
        public Ambiente Ambiente { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
