using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ItemCardapioRepository : BaseRepository<ItemCardapio>, IItemCardapioRepository
    {
        public ItemCardapioRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<ItemCardapio>> ObterAtivosAsync()
        {
            return await _dbSet
                .Where(i => i.Ativo)
                .ToListAsync();
        }

        public async Task<ItemCardapio?> ObterPorTipoAsync(TipoItem tipo)
        {
            return await _dbSet
                .FirstOrDefaultAsync(i => i.TipoItem == tipo);
        }
    }
}
