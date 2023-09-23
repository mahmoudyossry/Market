using Microsoft.EntityFrameworkCore;
using Market.Core.Entities.Base;
using Market.Core.IDto;
using Market.Core.Repositories.Base;
using Market.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MarketContext context;
        private readonly DbSet<T> dbSet;

        public Repository(MarketContext _econtext)
        {
            context = _econtext;
            dbSet = context.Set<T>();
        }
        public virtual async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<IList<T>> AddRangeAsync(IList<T> entity)
        {
            await dbSet.AddRangeAsync(entity);
            return entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual async Task DeleteAsync(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public virtual async Task<Tuple<ICollection<T>, int>> GetAllAsync(IPagingInputDto pagingInputDto)
        {
            var query = dbSet.AsQueryable();

            if (pagingInputDto.HiddenFilter != null)
            {
                query = query.Where(pagingInputDto.HiddenFilter);
            }

            if (pagingInputDto.Filter != null)
            {
                var props = typeof(T).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(FilterAttribute)));
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

            return new Tuple<ICollection<T>, int>(await page.ToListAsync(), total);
        }

        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        //public virtual IQueryable<T> AsQueryable()
        //{
        //    return dbSet.AsQueryable<T>();
        //}
    }
}
