using Novo.Models.Enums;

namespace Novo.Models.Domain
{
    public class Reserva
    {
        public Reserva(DateTime dataInicial, DateTime dataFinal, int idAmbiente, int idUsuario)
        {
            Status = Status.Reservado;
            IdAmbiente = idAmbiente;
            IdUsuario = idUsuario;
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
