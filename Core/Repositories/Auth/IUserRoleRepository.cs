using Market.Core.IDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Repositories
{
    public interface IUserRoleRepository<T> where T : class
    {
        Task<Tuple<ICollection<T>, int>> GetAllAsync(IPagingInputDto pagingInputDto);
        Task<T> GetByIdAsync(long id);
        Task<T> AddAsync(T entity);
        Task<IList<T>> AddRangeAsync(IList<T> entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(IEnumerable<T> entities);
        Task<string[]> GetUserRoleIdsByUserID(string id);
        Task<List<T>> GetRemovedUserRoleIdsByUserID(string id, string[] rolesIds);
    }
}
