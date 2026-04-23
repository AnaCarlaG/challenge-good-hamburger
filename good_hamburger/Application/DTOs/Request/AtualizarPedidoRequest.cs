using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class AtualizarPedidoRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "O pedido deve conter pelo menos um item.")]
        public List<TipoItem> Itens { get; set; }
    }
}
