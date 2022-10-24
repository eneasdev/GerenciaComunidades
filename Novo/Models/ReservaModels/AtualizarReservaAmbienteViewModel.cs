using Novo.Models.Enums;

namespace Novo.Models.ReservaModels
{
    public class AtualizarReservaAmbienteViewModel
    {
        public int IdReserva { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public bool Cancelar { get; set; }
        public bool Desativar { get; set; }
    }
}
