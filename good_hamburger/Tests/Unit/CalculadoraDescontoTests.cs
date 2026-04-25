using Domain.Entities;
using Domain.Enum;
using Domain.Util;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Unit
{
    public class CalculadoraDescontoTests
    {
        private static ItemPedido CriarPedido(TipoItem tipoItem, CategoriItem categoriItem, decimal preco) => new()
        {
            CategoriItem = categoriItem,
            TipoItem = tipoItem,
            Nome = tipoItem.ToString(),
            Preco = preco,
            CreatedAt = DateTime.Now
        };

        private static readonly ItemPedido XBurger = CriarPedido(TipoItem.XBurger, CategoriItem.Hamburguer, 5.00m);
        private static readonly ItemPedido XEgg = CriarPedido(TipoItem.XEgg, CategoriItem.Hamburguer, 4.50m);
        private static readonly ItemPedido XBacon = CriarPedido(TipoItem.XBacon, CategoriItem.Hamburguer, 7.00m);
        private static readonly ItemPedido BatataFrita = CriarPedido(TipoItem.BatatFrita, CategoriItem.Acompanhamento, 2.00m);
        private static readonly ItemPedido Refrigerante = CriarPedido(TipoItem.Refrigerante, CategoriItem.Bebida, 2.50m);

        [Fact]
        public void Calcular_SanduicheSozinho_SemDesconto() 
        {
            var itens = new List<ItemPedido> { XBurger };
            var (subtotal, percentual, desconto, total) = CalculadoraDesconto.CalcularDesconto(itens);

            percentual.Should().Be(0);
            desconto.Should().Be(0);
            subtotal.Should().Be(5.00m);
            total.Should().Be(5.00m);
        }
        [Fact]
        public void Calcular_SanduicheMaisBatata_Desconto10Porcento()
        {
            var itens = new List<ItemPedido> { XBurger, BatataFrita };
            var (subtotal, percentual, desconto, total) = CalculadoraDesconto.CalcularDesconto(itens);

            percentual.Should().Be(10);
            subtotal.Should().Be(7.00m);
            desconto.Should().Be(0.70m);
            total.Should().Be(6.30m);
        }
        [Fact]
        public void Calcular_SanduicheMaisRefrigerante_Desconto15Porcento()
        {
            var itens = new List<ItemPedido> { XEgg, Refrigerante };
            var (subtotal, percentual, desconto, total) = CalculadoraDesconto.CalcularDesconto(itens);

            percentual.Should().Be(15);
            subtotal.Should().Be(7.00m);
            desconto.Should().Be(1.05m);
            total.Should().Be(5.95m);
        }
        [Fact]
        public void Calcular_ComboCompleto_Desconto20Porcento()
        {
            var itens = new List<ItemPedido> { XBacon, BatataFrita, Refrigerante };
            var (subtotal, percentual, desconto, total) = CalculadoraDesconto.CalcularDesconto(itens);

            percentual.Should().Be(20);
            subtotal.Should().Be(11.50m);
            desconto.Should().Be(2.30m);
            total.Should().Be(9.20m);
        }
        [Fact]
        public void Calcular_ApenasAcompanhamentos_SemDesconto()
        {
            var itens = new List<ItemPedido> { BatataFrita, Refrigerante };
            var (subtotal, percentual, desconto, total) = CalculadoraDesconto.CalcularDesconto(itens);

            percentual.Should().Be(0);
            desconto.Should().Be(0);
            subtotal.Should().Be(4.50m);
            total.Should().Be(4.50m);
        }

    }
}
