using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Market.Core.Entities;
using Market.Core.Entities.Authorization;
using Market.Core.Entities.Base;
using Market.Infrastructure.DataConfiguration;
using Market.Core.Global;
using Market.Infrastructure.Identity;

namespace Market.Infrastructure.Data
{
    public class MarketContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly GlobalInfo globalInfo;

        private readonly UserRolesConfiguration userRolesConfiguration;
        private readonly PermissionConfiguration permissionConfiguration;
        private readonly RoleConfiguration roleConfiguration;
        private readonly RolePermissionConfiguration rolePermissionConfiguration;
        private readonly UserConfiguration userConfiguration;

        public MarketContext(DbContextOptions<MarketContext> options
                        , GlobalInfo globalInfo

                        , UserRolesConfiguration userRolesConfiguration
                        , RolePermissionConfiguration rolePermissionConfiguration
                        , RoleConfiguration roleConfiguration
                        , PermissionConfiguration permissionConfiguration
                        , UserConfiguration userConfiguration
            ) : base(options)
        {
            this.globalInfo = globalInfo;

            this.userRolesConfiguration = userRolesConfiguration;
            this.permissionConfiguration = permissionConfiguration;
            this.roleConfiguration = roleConfiguration;
            this.rolePermissionConfiguration = rolePermissionConfiguration;
            this.userConfiguration = userConfiguration;
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SetGlobalFilters(builder);
            ApplyConfigurations(builder);
        }

        #region SaveChanges 
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeSaveProccess();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            BeforeSaveProccess();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaveProccess();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        #endregion
        private void ApplyConfigurations(ModelBuilder builder)
        {
            builder.ApplyConfiguration(permissionConfiguration);
            builder.ApplyConfiguration(userConfiguration);
            builder.ApplyConfiguration(userRolesConfiguration);
            builder.ApplyConfiguration(roleConfiguration);
            builder.ApplyConfiguration(rolePermissionConfiguration);
        }

        private void SetGlobalFilters(ModelBuilder builder)
        {
            builder.SetQueryFilterOnAllEntities<ISoftDelete>(p => !p.IsDeleted);
        }

        private void BeforeSaveProccess()
        {
            var changes = from e in this.ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;

            foreach (var change in changes)
            {
                if (change.State == EntityState.Added)
                {
                    //if (change.Entity.GetType() == typeof(IAuditEntityx<>))
                    if (change.Entity is IAudit)
                    {
                        ((IAudit)change.Entity).CreateUser = globalInfo.UserId;
                        ((IAudit)change.Entity).CreateUserName = globalInfo.UserName;
                        ((IAudit)change.Entity).CreateDate = DateTime.Now;
                    }
                }
            }
        }
    }
}
