using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IItemCardapioRepository : IBaseRepository<ItemCardapio>
    {
        Task<IEnumerable<ItemCardapio>> ObterAtivosAsync();
        Task<ItemCardapio?> ObterPorTipoAsync(TipoItem tipo);
    }
}
