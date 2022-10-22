namespace Novo.Models.AmbienteModels
{
    public class AtualizarAmbienteViewModel
    {
        public int IdAmbiente { get; set; }
        public string Descricao { get; set; }
        public int QtdNovosItems { get; set; }
        public List<int> ListaItemsExcluidos { get; set; }
    }
}
