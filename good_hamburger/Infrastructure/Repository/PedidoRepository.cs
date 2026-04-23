using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<Pedido> AtualizarPedido(Pedido pedido)
        {
            var pedidoExistente = await _context.Pedidos
                                   .Include(p => p.ItemPedidos)
                                   .FirstOrDefaultAsync(p => p.Id == pedido.Id)
           ?? throw new NotFoundException(nameof(Pedido), pedido.Id);

            // remove itens antigos
            _context.Set<ItemPedido>().RemoveRange(pedidoExistente.ItemPedidos);

            // atualiza os dados
            pedidoExistente.ItemPedidos = pedido.ItemPedidos;
            pedidoExistente.Subtotal = pedido.Subtotal;
            pedidoExistente.Desconto = pedido.Desconto;
            pedidoExistente.PercentualDesconto = pedido.PercentualDesconto;
            pedidoExistente.Total = pedido.Total;
            pedidoExistente.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return pedidoExistente;
        }
        public async Task<Pedido?> ObterPorIdComItensAsync(Guid pedidoId)
        {
            return await _context.Pedidos
                .Include(p => p.ItemPedidos)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);
        }

        public async Task<IEnumerable<Pedido>> ObterTodosComItensAsync()
        {
            return await _context.Pedidos
                .Include(p => p.ItemPedidos)
                .ToListAsync();
        }
        public async Task<IEnumerable<Pedido>> ObterTodosComDeletedAsync()
        {
            return await _context.Pedidos
                .Include(p => p.ItemPedidos)
                .IgnoreQueryFilters()
                .ToListAsync();
        }
    }
}
