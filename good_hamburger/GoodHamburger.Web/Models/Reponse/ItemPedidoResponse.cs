using GoodHamburger.Web.Models.Enum;

namespace GoodHamburger.Web.Models.Reponse
{
    public class ItemPedidoResponse
    {
        public Guid Id { get; set; }
        public TipoItem TipoItem { get; set; }
        public CategoriaItem CategoriaItem { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}
