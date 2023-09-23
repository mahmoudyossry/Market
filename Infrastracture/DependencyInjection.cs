using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Market.Core.Repositories.Base;
using Market.Infrastructure.Data;
using Market.Infrastructure.Repositories;
using Market.Infrastructure.Repositories.Base;
using System.Configuration;
using Market.Core.UnitOfWork;
using Market.Infrastructure.UnitOfWorks;
using Market.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Market.Core.Global;
using Market.Core.Repositories;
using Market.Infrastructure.DataConfiguration;
using Market.Core;
using Market.Core.Entities;

namespace Market.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthInfrastructureDependencyInjection(this IServiceCollection services, AppSettingsConfiguration settings)
        {

            // For ERP Identity
            services.AddDbContext<MarketContext>(
                m => m.UseSqlServer(settings.ConnectionStrings.MarketDB));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<MarketContext>()
                //.AddUserManager<ApplicationUserManager>()
                .AddDefaultTokenProviders();

            // Overwrite the default password complixity
            services.Configure<IdentityOptions>(option => {
                option.Password.RequiredLength = 6;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireDigit = false;
                option.Password.RequireNonAlphanumeric = false;
                option.User.RequireUniqueEmail = false;
            }
            );

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped(typeof(IPermissionRepository), typeof(PermissionRepository));

            services.AddScoped(typeof(IRolePermissionRepository<RolePermission>), typeof(RolePermissionRepository));
            services.AddScoped(typeof(IUserRoleRepository<IdentityUserRole<string>>), typeof(UserRoleRepository));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ApplicationUserManager>();
            services.AddScoped<GlobalInfo>();
            services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));


            services.AddScoped<PermissionConfiguration>();
            services.AddScoped<RoleConfiguration>();
            services.AddScoped<RolePermissionConfiguration>();
            services.AddScoped<UserConfiguration>();
            services.AddScoped<UserRolesConfiguration>();

            
            return services;
        }
    }
}
