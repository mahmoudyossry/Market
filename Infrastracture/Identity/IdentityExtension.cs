using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Identity
{
    public static class IdentityExtensions
    {
        public static IdentityBuilder AddSecondIdentity<TUser, TRole>(
            this IServiceCollection services)
            where TUser : class
            where TRole : class
        {
            services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser, TRole>>();
            services.TryAddScoped<UserManager<TUser>, AspNetUserManager<TUser>>();
            services.TryAddScoped<SignInManager<TUser>, SignInManager<TUser>>();
            services.TryAddScoped<RoleManager<TRole>, AspNetRoleManager<TRole>>();
            services.TryAddScoped<IUserConfirmation<TUser>, DefaultUserConfirmation<TUser>>();

            return new IdentityBuilder(typeof(TUser), typeof(TRole), services);
        }

        //public static string Tenant(this IIdentity identity)
        //{
        //    var claim = ((ClaimsIdentity)identity).FindFirst("tenant");
        //    // Test for null to avoid issues during local testing
        //    return (claim != null) ? claim.Value : string.Empty;
        //}
    }

    //public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    //{
    //    public CustomClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
    //        IOptions<IdentityOptions> optionsAccessor)
    //        : base(userManager, roleManager, optionsAccessor)
    //    {
    //    }

    //    public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    //    {
    //        if (user == null)
    //            throw new ArgumentNullException(nameof(user));

    //        var principal = await base.CreateAsync(user);

    //        // Add your claims here
    //        ((ClaimsIdentity)principal.Identity).AddClaim(new Claim("tenant", user.TenantId.ToString()));

    //        return principal;
    //    }
    //}

}