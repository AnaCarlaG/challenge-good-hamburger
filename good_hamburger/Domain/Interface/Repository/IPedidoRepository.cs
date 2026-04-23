using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IPedidoRepository :IBaseRepository<Pedido>
    {
        Task<IEnumerable<Pedido>> ObterTodosComItensAsync();
        Task<IEnumerable<Pedido>> ObterTodosComDeletedAsync();
        Task<Pedido?> ObterPorIdComItensAsync(Guid pedidoId);
        Task<Pedido> AtualizarPedido(Pedido pedido);
    }
}
