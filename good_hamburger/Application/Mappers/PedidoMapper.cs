using Application.DTOs.Reponse;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Application.Mappers;
using System.Collections.Immutable;

namespace Application.Mappers
{
    public static class PedidoMapper
    {
        public static PedidoResponse ToResponse(this Pedido pedido) => new()
        {
            Id = pedido.Id,
            Status = pedido.Status,
            ItemPedidos = pedido.ItemPedidos.Select(i => i.ToResponse()).ToList(),
            Subtotal = pedido.Subtotal,
            Total = pedido.Total,
            Desconto = pedido.Desconto,
            PercentualDesconto = pedido.PercentualDesconto,
            CreatedAt = pedido.CreatedAt,
            UpdatedAt = pedido.UpdatedAt,
            DeletedAt = pedido.DeletedAt
        };

        public static ItemPedidoResponse ToResponse(this ItemPedido pedido) => new()
        {
            Id = pedido.Id,
            CategoriItem = pedido.CategoriItem,
            TipoItem = pedido.TipoItem,
            Nome = pedido.Nome,
            Preco = pedido.Preco
        };
    }
}
