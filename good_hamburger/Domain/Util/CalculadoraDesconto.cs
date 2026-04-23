using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Util
{
    public static class CalculadoraDesconto
    {
        public static (decimal Subtotal, decimal PercentualDesconto, decimal Desconto, decimal Total) CalcularDesconto(IEnumerable<ItemPedido> pedidos)
        {
            var listaPedidos = pedidos.ToList();

            var sanduiche = listaPedidos.Any(s => s.CategoriItem == CategoriItem.Hamburguer);
            var acompanhamento = listaPedidos.Any(s => s.CategoriItem == CategoriItem.Acompanhamento);
            var bebida = listaPedidos.Any(s => s.CategoriItem == CategoriItem.Bebida);

            var subTotal = listaPedidos.Sum(s => s.Preco);

            var percentual = (sanduiche, acompanhamento, bebida) switch
            {
                (true, true, true) => 20m,
                (true, false, true) => 15m,
                (true, true, false) => 10m,
                _ => 0m
            };

            var valorDesconto = subTotal * (percentual / 100);
            var total = subTotal - valorDesconto;
            return (subTotal, percentual, valorDesconto, total);
        }
    }
}
