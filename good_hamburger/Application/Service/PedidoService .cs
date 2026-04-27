using Application.DTOs.Reponse;
using Application.Mappers;
using Application.Service.Interface;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using Domain.Util;

namespace Application.Service
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IItemCardapioRepository _itemCardapioRepository;
        public PedidoService(IPedidoRepository pedidoRepository, IItemCardapioRepository itemCardapioRepository)
        {
            _pedidoRepository = pedidoRepository;
            _itemCardapioRepository = itemCardapioRepository;
        }

        public async Task<PedidoResponse> AcrescentarPedidoAsync(Guid id, IEnumerable<TipoItem> itens)
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(id)
                            ?? throw new NotFoundException(nameof(Pedido), id);
            var listaItens = itens.ToList();
            ValidarItensDuplicados(listaItens);
            var itensPedido = await ObterItensPedidoAsync(listaItens);
            var (subtotal, percentualDesconto, desconto, total) = CalculadoraDesconto.CalcularDesconto(itensPedido);

            pedido.ItemPedidos = itensPedido;
            pedido.Total = total;
            pedido.Subtotal = subtotal;
            pedido.Desconto = desconto;
            pedido.PercentualDesconto = percentualDesconto;

            var pedidoAtualizado = await _pedidoRepository.AtualizarAsync(pedido)
                    ?? throw new PedidoInvalidoException("Erro ao atualizar o pedido.", "O pedido retornou nulo após a atualização.");
            return pedidoAtualizado.ToResponse();
        }
        public async Task<PedidoResponse> AtualizarAsync(Guid id, IEnumerable<TipoItem> itens)
        {
            var listaItens = itens.ToList();
            ValidarItensDuplicados(listaItens);
            var itensPedido = await ObterItensPedidoAsync(listaItens);
            var (subtotal, percentualDesconto, desconto, total) = CalculadoraDesconto.CalcularDesconto(itensPedido);

            var pedido = new Pedido
            {
                Id = id,
                ItemPedidos = itensPedido,
                Total = total,
                Subtotal = subtotal,
                Desconto = desconto,
                PercentualDesconto = percentualDesconto
            };

            var pedidoAtualizado = await _pedidoRepository.AtualizarPedido(pedido)
                    ?? throw new PedidoInvalidoException("Erro ao atualizar o pedido.", "O pedido retornou nulo após a atualização.");
            return pedidoAtualizado.ToResponse();
        }

        public async Task<PedidoResponse> AtualizarStatusAsync(Guid id, StatusPedido status)
        {
            var pedido = await _pedidoRepository.ObterPorIdComItensAsync(id)
                            ?? throw new NotFoundException(nameof(PedidoResponse), id);

            pedido.Status = status;
            var statusAtualizado = await _pedidoRepository.AtualizarAsync(pedido)
                    ?? throw new PedidoInvalidoException("Erro ao atualizar o status do pedido.", "O pedido retornou nulo após a atualização.");
            return statusAtualizado.ToResponse();
        }

        public async Task<PedidoResponse> CriarAsync(IEnumerable<TipoItem> itens)
        {
            var listaItens = itens.ToList();

            ValidarItensDuplicados(listaItens);

            var itensPedido = await ObterItensPedidoAsync(listaItens);

            var (subtotal, percentualDesconto, desconto, total) = CalculadoraDesconto.CalcularDesconto(itensPedido);

            var pedido = new Pedido
            {
                Id = Guid.NewGuid(),
                ItemPedidos = itensPedido,
                Status = StatusPedido.Pendente,
                Subtotal = subtotal,
                PercentualDesconto = percentualDesconto,
                Desconto = desconto,
                Total = total,
                CreatedAt = DateTime.UtcNow
            };
            var pedidoCriado = await _pedidoRepository.CriarAsync(pedido)
                    ?? throw new PedidoInvalidoException("Erro ao criar o pedido.", "O pedido retornou nulo após a criação.");
            return pedidoCriado.ToResponse();
        }


        public async Task<PedidoResponse?> ObterPorIdAsync(Guid id)
        {
            var pedido = await _pedidoRepository.ObterPorIdComItensAsync(id)
                            ?? throw new NotFoundException(nameof(Pedido), id);
            return pedido.ToResponse();

        }

        public async Task<IEnumerable<PedidoResponse>> ObterTodosComItensAsync()
        {
            var pedido = await _pedidoRepository.ObterTodosComItensAsync();
            return pedido.Select(p => p.ToResponse());
        }

        public async Task RemoverAsync(Guid id, bool isDeleted = false)
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(id)
                            ?? throw new NotFoundException(nameof(Pedido), id);
            await _pedidoRepository.DeletarAsync(id, isDeleted);
        }

        private void ValidarItensDuplicados(List<TipoItem> itens)
        {
            var burguer = itens.Count(bur => bur == TipoItem.XEgg || bur == TipoItem.XBurger || bur == TipoItem.XBacon);
            var batata = itens.Count(bat => bat == TipoItem.BatatFrita);
            var refrigerante = itens.Count(refri => refri == TipoItem.Refrigerante);

            var itensErrados = new List<String>();
            if (burguer > 1)
            {
                itensErrados.Add("Sanduiche");

            }
            if (batata > 1)
            {
                itensErrados.Add("Batata-frita");
            }
            if (refrigerante > 1)
            {
                itensErrados.Add("Refrigerante");
            }
            if (itensErrados.Count > 0)
                throw new PedidoInvalidoException($"Os seguintes itens estão duplicados: {string.Join(", ", itensErrados)}");
        }

        private async Task<List<ItemPedido>> ObterItensPedidoAsync(IEnumerable<TipoItem> tipos)
        {
            var itensPedido = new List<ItemPedido>();
            foreach (var tipo in tipos)
            {
                var itemCardapio = await _itemCardapioRepository.ObterPorTipoAsync(tipo)
                                        ?? throw new NotFoundException(nameof(ItemCardapio), tipo.ToString());
                itensPedido.Add(new ItemPedido
                {
                    TipoItem = itemCardapio.TipoItem,
                    CategoriItem = itemCardapio.CategoriaItem,
                    Nome = itemCardapio.Nome,
                    Preco = itemCardapio.Preco,
                    CreatedAt = DateTime.UtcNow
                });
            }
            return itensPedido;
        }

        public async Task<IEnumerable<PedidoResponse>> ObterTodosComDeletedAsync()
        {
            var pedido = await _pedidoRepository.ObterTodosComDeletedAsync();
            return pedido.Select(p => p.ToResponse());
        }
    }
}
