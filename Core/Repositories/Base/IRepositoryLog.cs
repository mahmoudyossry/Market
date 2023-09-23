using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Repositories.Base
{
    public interface IRepositoryLog<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        IQueryable<T> AsQueryable();
        Task<T> GetByIdAsync(long id);
        Task<T> AddAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
