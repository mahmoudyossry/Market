using Microsoft.EntityFrameworkCore;
using Market.Core.Entities;
using Market.Core.Entities.Authorization;
using Market.Core.Repositories;
using Market.Infrastructure.Data;
using Market.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private readonly DbSet<Permission> dbSet;

        public PermissionRepository(MarketContext _econtext) : base(_econtext)
        {
            dbSet = context.Set<Permission>();
        }

        public async Task<List<Permission>> GetAllAsList()
        {
            return await dbSet.AsQueryable().ToListAsync();

        }
    }
}
