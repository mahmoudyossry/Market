using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Market.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Market.Core.Entities.Authorization;
using Market.Core.Entities;
using Market.Core.Global;

namespace Market.Infrastructure.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private UserStore<ApplicationUser, ApplicationRole, MarketContext, string, IdentityUserClaim<string>
            , IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>
            , IdentityRoleClaim<string>>
            _store;
        private readonly GlobalInfo globalInfo;

        public ApplicationUserManager(IUserStore<ApplicationUser> store,GlobalInfo globalInfo, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.globalInfo = globalInfo;
        }

        private MarketContext GetContext()
        {
            _store = (UserStore<ApplicationUser, ApplicationRole, MarketContext, string, IdentityUserClaim<string>,
                    IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>)this.Store;

            var context = _store.Context;
            return context;
        }

        public async Task<bool> UserHasAccess(string userId, string permmisions)
        {
            var context = GetContext();

            //check if curr user has admin role
            bool isAdminUser = await IsAdminAsync(userId);

            if (isAdminUser)
                return true;

            var permissionsArr = permmisions.Split(",").ToArray();
            var count = await context.UserRoles
                .Join(context.RolePermissions,
                    userRole => userRole.RoleId,
                    rolePermission => rolePermission.RoleId,
                    (userRole, rolePermission) => new { userRole, rolePermission })
                .Join(context.Permissions,
                    rp => rp.rolePermission.PermissionId,
                    permission => permission.Id,
                    (rp, permission) => new { rp, permission })
                .Where(x => permissionsArr.Any(a => a == x.permission.Name) && x.rp.userRole.UserId == userId)
                .CountAsync();

            return count > 0;
        }

        public async Task<string[]> GetUserPermission(string userId)
        {
            var context = GetContext();
            string[] permissions = Array.Empty<string>();

            //check if curr user has admin role
            bool isAdminUser = await IsAdminAsync(userId);

            //if curr user has admin role, return all available permissions for curr tenant
            if (isAdminUser)
            {
                    permissions = context.Permissions
                        .Select(xx => xx.Name).ToArray();
            }
            //if not admin then return only assigned permissions
            else
            {
                    permissions = await context.UserRoles
                    .Join(context.RolePermissions,
                        userRole => userRole.RoleId,
                        rolePermission => rolePermission.RoleId,
                        (userRole, rolePermission) => new { userRole, rolePermission })
                    .Join(context.Permissions,
                        rp => rp.rolePermission.PermissionId,
                        permission => permission.Id,
                        (rp, permission) => new { rp, permission })
                    .Where(x => x.rp.userRole.UserId == userId)
                    .Select(x => x.permission.Name)
                    .AsNoTracking().ToArrayAsync();
            }
            return permissions;
        }

        private async Task<bool> IsAdminAsync(string userId)
        {
            var context = GetContext();
            return await context.Roles
                    .Join(context.UserRoles,
                        role => role.Id,
                        userRole => userRole.RoleId,
                        (role, userRole) => new { role, userRole })
                    .Where(x => x.userRole.UserId == userId && x.role.IsAdmin == true)
                    .CountAsync() > 0;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string Id)
        {
            var context = GetContext();
            return await context.Users.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<ApplicationUser> GetUserByPhoneAsync(string phoneNumber)
        {
            var context = GetContext();
            return await context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        }
        public async Task<bool> UserIsExist(string Id)
        {
            var context = GetContext();
            return await context.Users.AnyAsync(x => x.Id == Id);
        }
        public async Task<string> GetUserNameByIdAsync(string Id)
        {
            var context = GetContext();
            var user= await context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            return user?.FullName;
        }

        public async Task<(ApplicationUser, string[])> GetUserSession(string userId)
        {
            var context = GetContext();

            var permissions = await GetUserPermission(userId);

            var user = await context.Users
                .Include(x=>x.UserRoles)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            return (user, permissions);
        }
        public async Task<ApplicationUser[]> GetUsersPermission(string permission)
        {
            var context = GetContext();
            return await context.Users
                .Where(x => x.UserRoles
                .Any(r => r.RolePermissions
                .Any(p => p.Permission.Name.Equals(permission))))
                .ToArrayAsync();
        }

        public async Task<bool> IsStopped(string userId)
        {
            var context = GetContext();
            var user= await context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);
            return !user.IsActive;
        }

    }
}
