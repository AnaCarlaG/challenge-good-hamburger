using Application.Service;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using Moq;

namespace Tests.Unit
{
    public class PedidoServiceTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepository;
        private readonly Mock<IItemCardapioRepository> _itemCardapioRepository;
        private readonly PedidoService _service;

        public PedidoServiceTests()
        {
            _pedidoRepository = new Mock<IPedidoRepository>();
            _itemCardapioRepository = new Mock<IItemCardapioRepository>();

            _service = new PedidoService(_pedidoRepository.Object, _itemCardapioRepository.Object);
        }

        // ─── helpers ───────────────────────────────────────────────
        private static ItemCardapio MakeCardapio(TipoItem tipo, CategoriItem categoriItem, decimal preco) => new()
        {
            TipoItem = tipo,
            CategoriItem = categoriItem,
            Preco = preco,
            Nome = tipo.ToString()
        };

        private void SetupCardapio()
        {
            _itemCardapioRepository.Setup(r => r.ObterPorTipoAsync(TipoItem.XBurger)).ReturnsAsync(MakeCardapio(TipoItem.XBurger, CategoriItem.Hamburguer, 5.00m));
            _itemCardapioRepository.Setup(r => r.ObterPorTipoAsync(TipoItem.XEgg)).ReturnsAsync(MakeCardapio(TipoItem.XEgg, CategoriItem.Hamburguer, 4.50m));
            _itemCardapioRepository.Setup(r => r.ObterPorTipoAsync(TipoItem.XBacon)).ReturnsAsync(MakeCardapio(TipoItem.XBacon, CategoriItem.Hamburguer, 7.00m));
            _itemCardapioRepository.Setup(r => r.ObterPorTipoAsync(TipoItem.BatatFrita)).ReturnsAsync(MakeCardapio(TipoItem.BatatFrita, CategoriItem.Acompanhamento, 2.00m));
            _itemCardapioRepository.Setup(r => r.ObterPorTipoAsync(TipoItem.Refrigerante)).ReturnsAsync(MakeCardapio(TipoItem.Refrigerante, CategoriItem.Bebida, 2.50m));
        }

        private static Pedido MakePedido(Guid? id = null, StatusPedido status = StatusPedido.Pendente) => new()
        {
            Id = id ?? Guid.NewGuid(),
            Status = status,
            ItemPedidos = new List<ItemPedido>()
        };

        // ─── CriarAsync ────────────────────────────────────────────
        [Fact]
        public async Task CriarAsync_ComItensValidos_RetornaPedidoCriado()
        {
            SetupCardapio();
            var pedidoCriado = new Pedido { Id = Guid.NewGuid(), ItemPedidos = new List<ItemPedido>() };

            _pedidoRepository.Setup(r => r.CriarAsync(It.IsAny<Pedido>())).ReturnsAsync(pedidoCriado);

            var result = await _service.CriarAsync(new[] { TipoItem.XBurger });

            Assert.NotNull(result);
        }
        [Fact]
        public async Task CriarAsync_ComDoisSanduiches_LancaPedidoInvalidoException()
        {
            SetupCardapio();
            var itens = new[] { TipoItem.XBurger, TipoItem.XEgg };

            await Assert.ThrowsAsync<PedidoInvalidoException>(() => _service.CriarAsync(itens));
        }
        [Fact]
        public async Task CriarAsync_ComDuasBatatas_LancaPedidoInvalidoException()
        {
            SetupCardapio();
            var itens = new[] { TipoItem.XBurger, TipoItem.BatatFrita, TipoItem.BatatFrita };

            await Assert.ThrowsAsync<PedidoInvalidoException>(() => _service.CriarAsync(itens));
        }
        [Fact]
        public async Task CriarAsync_ComDoisRefrigerantes_LancaPedidoInvalidoException()
        {
            SetupCardapio();
            var itens = new[] { TipoItem.XBurger, TipoItem.Refrigerante, TipoItem.Refrigerante };

            await Assert.ThrowsAsync<PedidoInvalidoException>(() => _service.CriarAsync(itens));
        }
        [Fact]
        public async Task CriarAsync_QuandoRepositorioRetornaNulo_LancaPedidoInvalidoException()
        {
            SetupCardapio();
            var pedidoCriado = new Pedido { Id = Guid.NewGuid() };

            _pedidoRepository.Setup(r => r.CriarAsync(It.IsAny<Pedido>())).ReturnsAsync((Pedido?)null);

            await Assert.ThrowsAsync<PedidoInvalidoException>(() => _service.CriarAsync(new[] { TipoItem.XBurger }));
        }
        // ─── ObterPorIdAsync ────────────────────────────────────────
        [Fact]
        public async Task ObterPorIdAsync_QuandoPedidoNaoExiste_LancaNotFoundException()
        {
            var id = Guid.NewGuid();
            _pedidoRepository.Setup(r => r.ObterPorIdComItensAsync(id))
                                           .ReturnsAsync((Pedido?)null);
            await Assert.ThrowsAsync<NotFoundException>(() => _service.ObterPorIdAsync(id));
        }
        // ─── RemoverAsync ───────────────────────────────────────────
        [Fact]
        public async Task RemoverAsync_QuandoPedidoNaoExiste_LancaNotFoundException()
        {
            var id = Guid.NewGuid();
            _pedidoRepository.Setup(r => r.ObterPorIdAsync(id))
                                           .ReturnsAsync((Pedido?)null);
            await Assert.ThrowsAsync<NotFoundException>(() => _service.RemoverAsync(id));
        }
        [Fact]
        public async Task RemoverAsync_QuandoPedidoNaoExisteRemoverAsync_QuandoPedidoExiste_ChamaDeletarAsync_LancaNotFoundException()
        {
            var id = Guid.NewGuid();
            _pedidoRepository.Setup(r => r.ObterPorIdAsync(id))
                                           .ReturnsAsync(new Pedido { Id = id });
            await _service.RemoverAsync(id);
            _pedidoRepository.Verify(r => r.DeletarAsync(id, false), Times.Once);
        }
        // ─── AtualizarAsync ────────────────────────────────────────
        [Fact]
        public async Task AtualizarAsync_ComItensValidos_RetornaPedidoAtualizado()
        {
            SetupCardapio();
            var id = Guid.NewGuid();
            var pedidoAtualizado = new Pedido { Id = id, ItemPedidos = new List<ItemPedido>() };
            _pedidoRepository.Setup(r => r.AtualizarPedido(It.IsAny<Pedido>())).ReturnsAsync(pedidoAtualizado);

            var result = await _service.AtualizarAsync(id, new[] { TipoItem.XBurger, TipoItem.BatatFrita });
            Assert.NotNull(result);
        }
        [Fact]
        public async Task AtualizarAsync_ComDoisSanduiches_LancaPedidoInvalidoException()
        {
            var itens = new[] { TipoItem.XBurger, TipoItem.XEgg };
            await Assert.ThrowsAsync<PedidoInvalidoException>(() => _service.AtualizarAsync(Guid.NewGuid(), itens));
        }
        [Fact]
        public async Task AtualizarAsync_ComDuasBatatas_LancaPedidoInvalidoException()
        {
            var itens = new[] { TipoItem.XBurger, TipoItem.BatatFrita, TipoItem.BatatFrita };
            await Assert.ThrowsAsync<PedidoInvalidoException>(() => _service.AtualizarAsync(Guid.NewGuid(), itens));
        }
        [Fact]
        public async Task AtualizarAsync_QuandoRepositorioRetornaNulo_LancaPedidoInvalidoException()
        {
            SetupCardapio();
            _pedidoRepository.Setup(r => r.AtualizarPedido(It.IsAny<Pedido>())).ReturnsAsync((Pedido?)null);

            await Assert.ThrowsAsync<PedidoInvalidoException>(() => _service.AtualizarAsync(Guid.NewGuid(), new[] { TipoItem.XBurger }));

        }
        [Fact]
        public async Task AtualizarAsync_ComCombinacaoCompleta_CalculaDescontoCorreto()
        {
            SetupCardapio();
            Pedido? pedidoCapturado = null;
            var id = Guid.NewGuid();

            _pedidoRepository.Setup(r => r.AtualizarPedido(It.IsAny<Pedido>()))
                .Callback<Pedido>(p => pedidoCapturado = p)
                .ReturnsAsync(MakePedido(id));
            await _service.AtualizarAsync(id, new[]
                {
                TipoItem.XBurger, TipoItem.BatatFrita, TipoItem.Refrigerante
            });

            Assert.NotNull(pedidoCapturado);
            Assert.Equal(9.50m, pedidoCapturado!.Subtotal);
            Assert.Equal(20m, pedidoCapturado.PercentualDesconto);
            Assert.Equal(1.90m, pedidoCapturado.Desconto);
            Assert.Equal(7.60m, pedidoCapturado.Total);
        }
        // ─── AcrescentarPedidoAsync ────────────────────────────────
        public async Task AcrescentarPedidoAsync_QuandoPedidoNaoExiste_LancaNotFoundException()
        {
            var id = Guid.NewGuid();
            _pedidoRepository.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync((Pedido?)null);
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _service.AcrescentarPedidoAsync(id, new[] { TipoItem.XBurger }));
        }
        [Fact]
        public async Task AcrescentarPedidoAsync_ComItensValidos_AtualizaERetorna()
        {
            SetupCardapio();
            var id = Guid.NewGuid();
            _pedidoRepository
                .Setup(r => r.ObterPorIdAsync(id))
                .ReturnsAsync(new Pedido { Id = id });
            _pedidoRepository
                .Setup(r => r.AtualizarAsync(It.IsAny<Pedido>()))
                .ReturnsAsync(new Pedido { Id = id, ItemPedidos = new List<ItemPedido>() });
            var result = await _service.AcrescentarPedidoAsync(id, new[] { TipoItem.XBurger });

            Assert.NotNull(result);
            _pedidoRepository.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Once);
        }
        [Fact]
        public async Task AcrescentarPedidoAsync_ComItensDuplicados_LancaPedidoInvalidoException()
        {
            var id = Guid.NewGuid();
            _pedidoRepository
                .Setup(r => r.ObterPorIdAsync(id))
                .ReturnsAsync(new Pedido { Id = id });

            await Assert.ThrowsAnyAsync<PedidoInvalidoException>(() => _service.AcrescentarPedidoAsync(id, new[] { TipoItem.XBurger, TipoItem.XBacon }));
        }
        [Fact]
        public async Task AcrescentarPedidoAsync_QuandoAtualizarRetornaNulo_LancaPedidoInvalidoException()
        {
            SetupCardapio();
            var id = Guid.NewGuid();
            _pedidoRepository
                .Setup(r => r.ObterPorIdAsync(id))
                .ReturnsAsync(new Pedido { Id = id });
            _pedidoRepository
                .Setup(r => r.AtualizarAsync(It.IsAny<Pedido>()))
                .ReturnsAsync((Pedido?)null);

            await Assert.ThrowsAnyAsync<PedidoInvalidoException>(() => _service.AcrescentarPedidoAsync(id, new[] { TipoItem.XBurger }));
        }
        [Fact]
        public async Task AcrescentarPedidoAsync_ComCombinacaoCompleta_CalculaDescontoCorreto()
        {
            SetupCardapio();
            var id = Guid.NewGuid();
            Pedido? pedidoCapturado = null;

            _pedidoRepository
                .Setup(r => r.ObterPorIdAsync(id))
                .ReturnsAsync(new Pedido { Id = id });
            _pedidoRepository
                .Setup(r => r.AtualizarAsync(It.IsAny<Pedido>()))
                .Callback<Pedido>(p => pedidoCapturado = p)
                .ReturnsAsync(MakePedido(id));

            await _service.AcrescentarPedidoAsync(id, new[]
            {
                TipoItem.XBurger, TipoItem.BatatFrita,TipoItem.Refrigerante
            });
            Assert.NotNull(pedidoCapturado);
            Assert.Equal(9.50m, pedidoCapturado!.Subtotal);
            Assert.Equal(20m, pedidoCapturado.PercentualDesconto);
            Assert.Equal(7.60m, pedidoCapturado.Total);
        }
        // ─── AtualizarStatusAsync ──────────────────────────────────
        [Fact]
        public async Task AtualizarStatusAsync_QuandoPedidoNaoExiste_LancaNotFoundException()
        {
            var id = Guid.NewGuid();
            _pedidoRepository.Setup(r => r.ObterPorIdComItensAsync(id))
                .ReturnsAsync((Pedido?)null);
            await Assert.ThrowsAsync<NotFoundException>(() => _service.AtualizarStatusAsync(id, StatusPedido.EmPreparacao));
        }
        [Fact]
        public async Task AtualizarStatusAsync_ComStatusValido_AtualizaStatus()
        {
            var id = Guid.NewGuid();
            var pedido = new Pedido { Id = id ,Status = StatusPedido.Pendente};

            _pedidoRepository.Setup(r => r.ObterPorIdComItensAsync(id)).ReturnsAsync(pedido);
            _pedidoRepository.Setup(r => r.AtualizarAsync(It.IsAny<Pedido>()))
                .ReturnsAsync(MakePedido(id));
            var result = await _service.AtualizarStatusAsync(id, StatusPedido.EmPreparacao);

            Assert.NotNull(result);
            _pedidoRepository.Verify(r => r.AtualizarAsync(It.Is<Pedido>(p => p.Status == StatusPedido.EmPreparacao)), Times.Once);
        }
        [Fact]
        public async Task AtualizarStatusAsync_QuandoAtualizarRetornaNulo_LancaPedidoInvalidoException()
        {
            var id = Guid.NewGuid();

            _pedidoRepository.Setup(r => r.ObterPorIdComItensAsync(id)).ReturnsAsync(new Pedido { Id = id});
            _pedidoRepository.Setup(r => r.AtualizarAsync(It.IsAny<Pedido>()))
                .ReturnsAsync((Pedido?)null);
            await Assert.ThrowsAsync<PedidoInvalidoException>( () => _service.AtualizarStatusAsync(id, StatusPedido.Pronto));
        }
        [Theory]
        [InlineData(StatusPedido.Pendente)]
        [InlineData(StatusPedido.EmPreparacao)]
        [InlineData(StatusPedido.Pronto)]
        [InlineData(StatusPedido.Entregue)]
        [InlineData(StatusPedido.Cancelado)]
        public async Task AtualizarStatusAsync_ParaCadaStatus_AtualizaCorretamente(StatusPedido statusPedido)
        { 
            var id = Guid.NewGuid();
            _pedidoRepository.Setup(r => r.ObterPorIdComItensAsync(id)).ReturnsAsync(MakePedido(id));
            _pedidoRepository.Setup(r => r.AtualizarAsync(It.IsAny<Pedido>())).ReturnsAsync(new Pedido { Id = id, Status = statusPedido, ItemPedidos = new List<ItemPedido>() });

            var result = await _service.AtualizarStatusAsync(id, statusPedido);

            Assert.NotNull(result);
        }
    }
}
