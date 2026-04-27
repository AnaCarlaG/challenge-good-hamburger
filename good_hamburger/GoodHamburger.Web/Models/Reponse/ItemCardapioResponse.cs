using GoodHamburger.Web.Models.Enum;
using System.Text.Json.Serialization;

namespace GoodHamburger.Web.Models.Reponse
{
    public class ItemCardapioResponse
    {
        public Guid Id { get; set; }
        public TipoItem TipoItem { get; set; }
        public CategoriaItem CategoriaItem { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
    }
}
