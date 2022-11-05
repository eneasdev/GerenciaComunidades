using Microsoft.AspNetCore.Mvc.Rendering;
using Novo.Models.Domain;

namespace Novo.Models.ReservaModels
{
    public class ListarItemsViewModel
    {
        public int IdAmbiente { get; set; }
        public SelectList Ambientes { get; set; }
    }
}
