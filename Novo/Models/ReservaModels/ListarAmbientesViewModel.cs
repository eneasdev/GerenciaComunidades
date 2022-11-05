using Novo.Models.Enums;

namespace Novo.Models.ReservaModels
{
    public class ListarAmbientesViewModel
    {
        public int IdAmbiente { get; set; }
        public string Descricao { get; set; }
        public Status Status { get; set; }
        public int QtdItens { get; set; }
    }
}
