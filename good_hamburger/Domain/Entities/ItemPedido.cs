using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ItemPedido : BaseEntity
    {
        public Guid PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public TipoItem TipoItem { get; set; }
        public CategoriaItem CategoriItem { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}
