using Novo.Models.Enums;

namespace Novo.Models.Domain
{
    public class Ambiente
    {
        public Ambiente(string descricao, Status status = Status.Ativo)
        {
            Descricao = descricao;
            Status = status;
            Items = new HashSet<Item>();
        }

        public int IdAmbiente { get; set; }
        public string Descricao { get; set; }
        public Status Status { get; set; }

        public ICollection<Item>? Items { get; set; }

        public void CriarAcentos(int? quantidade)
        {
            if (quantidade.HasValue)
            {
                for (int i = 0; i < quantidade; i++)
                {
                    var descricao = $"Item {i} ";
                    Items.Add(new Item(descricao, Status.Ativo));
                }
            }
        }

        public void Desativar()
        {
            this.Status = Status.Desativado;
        }
    }
}
