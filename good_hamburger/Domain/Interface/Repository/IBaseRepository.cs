using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> ObterTodosAsync();
        Task<T?> ObterPorIdAsync(Guid id);
        Task<T> CriarAsync(T entity);
        Task<T> AtualizarAsync(T entity);
        Task DeletarAsync(Guid id, bool isDeleted = false);
    }
}
