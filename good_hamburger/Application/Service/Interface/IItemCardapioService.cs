using Application.DTOs.Reponse;
using Domain.Entities;

namespace Application.Service.Interface
{
    public interface IItemCardapioService
    {
        Task<IEnumerable<ItemCardapioResponse>> ObterTodosAsync();
        Task<ItemCardapioResponse> ObterPorIdAsync(Guid id);
    }
}
