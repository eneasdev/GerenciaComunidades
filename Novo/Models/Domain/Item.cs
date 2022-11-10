using Novo.Models.Enums;

namespace Novo.Models.Domain
{
    public class Item
    {
        public Item(string descricao, Status status)
        {
            Descricao = descricao;
            Status = status;
        }

        public int IdItem { get; set; }
        public string Descricao { get; set; }
        public Status Status { get; set; }


        public int IdAmbiente { get; set; }
        public virtual Ambiente Ambiente { get; set; }
    }
}
