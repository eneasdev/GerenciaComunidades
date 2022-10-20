using Novo.Models.Enums;

namespace Novo.Models.ReservaModels
{
    public class ReservaUsuarioModel
    {

        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public int IdAmbiente { get; set; }
        public string Descricao { get; set; }
        public Status Status { get; set; }
        public int IdUsuario { get; set; }  
    }
}
