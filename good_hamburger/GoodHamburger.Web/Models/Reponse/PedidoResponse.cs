using GoodHamburger.Web.Models.Enum;

namespace GoodHamburger.Web.Models.Reponse
{
    public class PedidoResponse
    {
        public Guid Id { get; set; }
        public IEnumerable<ItemPedidoResponse> ItemPedidos { get; set; }
        public StatusPedido Status { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal PercentualDesconto { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt {  get; set; }
        public DateTime? UpdatedAt {  get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
