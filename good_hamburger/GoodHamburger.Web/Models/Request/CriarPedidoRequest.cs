using GoodHamburger.Web.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Web.Models.Request
{
    public class CriarPedidoRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "O pedido deve conter pelo menos um item.")]
        public List<TipoItem> Itens {  get; set; }
    }
}
