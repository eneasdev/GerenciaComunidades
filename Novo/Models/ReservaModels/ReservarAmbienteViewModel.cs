using Novo.Models.Enums;

namespace Novo.Models.ReservaModels
{
    public class ReservarAmbienteViewModel
    {
        public string Dia { get; set; }
        public string Periodo { get; set; }
        public int IdAmbiente { get; set; }
        public string Descricao { get; set; }
        public Status Status { get; set; }
        public int IdUsuario { get; set; }
    }
}
