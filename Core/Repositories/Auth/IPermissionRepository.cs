using Market.Core.Entities.Authorization;
using Market.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<List<Permission>> GetAllAsList();

    }
}
