using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Pedido : BaseEntity
    {
        public IEnumerable<ItemPedido> ItemPedidos { get; set; }
        public StatusPedido Status { get; set; }
        public decimal Subtotal { get; set; }  
        public decimal Desconto { get; set; }  
        public decimal PercentualDesconto { get; set; }
        public decimal Total { get; set; }

        public Pedido()
        {
            Id = Guid.NewGuid();
            Status = StatusPedido.Pendente;
            CreatedAt = DateTime.Now;
        }

    }
}
