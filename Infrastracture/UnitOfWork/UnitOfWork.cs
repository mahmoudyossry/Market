using Microsoft.AspNetCore.Identity;
using Market.Core.Entities;
using Market.Core.Entities.Authorization;
using Market.Core.Repositories;
using Market.Core.Repositories.Base;
using Market.Core.UnitOfWork;
using Market.Infrastructure.Data;
using Market.Core.Global;
using Market.Infrastructure.Identity;
using Market.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MarketContext context;

        public IPermissionRepository Permission { get; }
        public IRepository<Product> Product { get; }

        public UnitOfWork(MarketContext _context
            , IPermissionRepository Permission
            , IRepository<Product> Product

            )
        {
            context = _context;
            this.Permission = Permission;
            this.Product = Product;
        }

        public async Task<int> CompleteAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                RollbackTran();
                throw ex;
            }

        }

        public object GetRepositoryByName(string name)
        {
            Type type = this.GetType();
            PropertyInfo info = type.GetProperty(name);
            if (info == null)
                throw new AppException(ExceptionEnum.PropertyNotAccess, name, type.FullName);
            //type.FullName, String.Format("A property called {0} can't be accessed for type {1}.", name));

            return info.GetValue(this, null);
        }

        public void BeginTran()
        {
            context.Database.BeginTransaction();
        }

        public void CommitTran()
        {
            context.Database.CommitTransaction();
        }

        public void RollbackTran()
        {
            var transaction = context.Database.CurrentTransaction;
            if (transaction != null)
                context.Database.RollbackTransaction();
        }
    }
}
