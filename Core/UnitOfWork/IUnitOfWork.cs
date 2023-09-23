using Market.Core.Entities;
using Market.Core.Entities.Authorization;
using Market.Core.Repositories;
using Market.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPermissionRepository Permission { get; }       
        IRepository<Product> Product { get; }

        Task<int> CompleteAsync();

        void BeginTran();

        void CommitTran();

        void RollbackTran();

        object GetRepositoryByName(string name);
    }
}
