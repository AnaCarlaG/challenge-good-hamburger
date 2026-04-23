using Application.DTOs.Reponse;
using Domain.Entities;
using Domain.Enum;

namespace Application.Service.Interface
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoResponse>> ObterTodosComItensAsync();
        Task<IEnumerable<PedidoResponse>> ObterTodosComDeletedAsync();
        Task<PedidoResponse?> ObterPorIdAsync(Guid id);
        Task<PedidoResponse> CriarAsync(IEnumerable<TipoItem> itens);
        Task<PedidoResponse> AcrescentarPedidoAsync(Guid id, IEnumerable<TipoItem> itens);
        Task<PedidoResponse> AtualizarAsync(Guid id, IEnumerable<TipoItem> itens);
        Task<PedidoResponse> AtualizarStatusAsync(Guid id, StatusPedido status);
        Task RemoverAsync(Guid id, bool isDeleted = false);
    }
}
