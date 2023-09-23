using Microsoft.EntityFrameworkCore;
using Market.Core.Entities.Base;
using Market.Core.IDto;
using Market.Core.Repositories;
using Market.Infrastructure.Data;
using Market.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository<RolePermission>
    {
        protected readonly MarketContext context;
        private readonly DbSet<RolePermission> dbSet;

        public RolePermissionRepository(MarketContext _econtext)
        {
            context = _econtext; 
            dbSet = context.Set<RolePermission>();
        }
        public virtual async Task<RolePermission> AddAsync(RolePermission entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<IList<RolePermission>> AddRangeAsync(IList<RolePermission> entity)
        {
            await dbSet.AddRangeAsync(entity);
            return entity;
        }

        public virtual async Task DeleteAsync(RolePermission entity)
        {
            dbSet.Remove(entity);
        }

        public virtual async Task DeleteAsync(IEnumerable<RolePermission> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public virtual async Task<Tuple<ICollection<RolePermission>, int>> GetAllAsync(IPagingInputDto pagingInputDto)
        {
            var query = dbSet.AsQueryable();

            if (pagingInputDto.Filter != null)
            {
                var props = typeof(RolePermission).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(FilterAttribute)));
                var condition = "";
                foreach (var p in props)
                {
                    condition = (condition == "" ? condition : condition + " || ") + p.Name + ".Contains(@0)";
                }

                query = query.Where(condition, pagingInputDto.Filter);
            }

            var order = query.OrderBy(pagingInputDto.OrderByField + " " + pagingInputDto.OrderType);

            var page = order.Skip((pagingInputDto.PageNumber * pagingInputDto.PageSize) - pagingInputDto.PageSize).Take(pagingInputDto.PageSize);

            var total = await query.CountAsync();

            return new Tuple<ICollection<RolePermission>, int>(await page.ToListAsync(), total);
        }

        public virtual async Task<RolePermission> GetByIdAsync(long id)
        {
            return await context.Set<RolePermission>().FindAsync(id);
        }

        public async Task<string[]> GetRolePermissionsIdsByRoleID(string id)
        {
            return await dbSet.AsQueryable()
                .Where(x => x.RoleId == id).Select(x => x.PermissionId.ToString()).ToArrayAsync();
        }
        public async Task<List<RolePermission>> GetRemovedRolePermissionsIdsByRoleID(string id, string[] permissionIds)
        {
            return await dbSet.AsQueryable()
               .Where(x => x.RoleId == id && !permissionIds.Contains(x.PermissionId.ToString())).ToListAsync();
        }
    }
}

