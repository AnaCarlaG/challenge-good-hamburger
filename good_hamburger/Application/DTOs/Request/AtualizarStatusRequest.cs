using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class AtualizarStatusRequest
    {
        [Required]
        public StatusPedido Status { get; set; }
    }
}
