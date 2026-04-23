using Application.DTOs.Reponse;
using Application.Mappers;
using Application.Service.Interface;
using Domain.Entities;
using Domain.Interfaces.Repository;

namespace Application.Service
{
    public class ItemCardapioService : IItemCardapioService
    {
        private readonly IItemCardapioRepository _itemCardapioRepository;

        public ItemCardapioService(IItemCardapioRepository itemCardapioRepository)
        {
            _itemCardapioRepository = itemCardapioRepository;
        }

        public async Task<ItemCardapioResponse> ObterPorIdAsync(Guid id)
        {
            var item = await _itemCardapioRepository.ObterPorIdAsync(id);
            return item.ToResponse();
        }

        public async Task<IEnumerable<ItemCardapioResponse>> ObterTodosAsync()
        {
            var item = await _itemCardapioRepository.ObterTodosAsync();
            return item.Select(i => i.ToResponse());
        }
    }
}
