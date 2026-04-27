using GoodHamburger.Web.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Web.Models.Request
{
    public class AtualizarStatusRequest
    {
        [Required]
        public StatusPedido Status { get; set; }
    }
}
