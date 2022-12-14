using Novo.Models.Enums;

namespace Novo.Models.Domain
{
    public class Reserva
    {
        public Reserva(DateTime dataInicial, DateTime dataFinal, string idUsuario, int? idAmbiente = null, int? idItem = null)
        {
            Status = StatusReserva.Reservado;
            IdAmbiente = idAmbiente;
            IdItem = idItem;
            IdUsuario = idUsuario;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
        }

        public int IdReserva { get; set; }
        public StatusReserva Status { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }

        public int? IdAmbiente { get; set; }
        public Ambiente? Ambiente { get; set; }
        public int? IdItem { get; set; }
        public Item? Item { get; set; }
        public string IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public Reserva Reservar(DateTime dataInicial, DateTime dataFinal, string idUsuario, int idAmbiente, int idItem)
        {
            return new Reserva(dataInicial, dataFinal, idUsuario, idAmbiente, idItem);
        }

        public void Atualizar(DateTime? dataInicial, DateTime? dataFinal)
        {
            this.DataInicial = dataInicial ?? this.DataInicial;
            this.DataFinal = dataFinal ?? this.DataFinal;
        }
    }
}
