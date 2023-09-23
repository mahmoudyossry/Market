using Microsoft.Extensions.DependencyInjection;
using Market.Application;
using Market.Application.Services;
using Market.Application.Services.Interfaces;
using Market.Core;

namespace Market.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthApplicationDependencyInjection(this IServiceCollection services, AppSettingsConfiguration config)
        {
            services.AddSingleton(config);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped(typeof(IService<,,>), typeof(BaseService<,,>));
            services.AddScoped(typeof(IFileService), typeof(FileService));
            //services.AddSingleton(typeof(AppSettingsConfiguration));

            return services;
        }
    }
}
