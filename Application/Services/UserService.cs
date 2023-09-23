using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Market.Application.Dto;
using Market.Application.Mapper;
using Market.Application.Services.Interfaces;
using Market.Core;
using Market.Core.Entities.Base;
using Market.Core.Global;
using Market.Core.Repositories;
using Market.Core.UnitOfWork;
using Market.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web;

namespace Market.Application.Services
{
    public class UserService : IUserService
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationUserManager userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRoleRepository<IdentityUserRole<string>> userRole;
        private readonly GlobalInfo globalInfo;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly AppSettingsConfiguration appSettings;

        public UserService(ApplicationUserManager userManager
            , RoleManager<ApplicationRole> roleManager
            , IUnitOfWork unitOfWork
            , IUserRoleRepository<IdentityUserRole<string>> userRole
            , GlobalInfo globalInfo
            ,IPasswordHasher<ApplicationUser> passwordHasher
            ,AppSettingsConfiguration appSettings
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
            this.userRole = userRole;
            this.globalInfo = globalInfo;
            this.passwordHasher = passwordHasher;
            this.appSettings = appSettings;
        }
        public virtual async Task<PagingResultDto<UserAllDto>> GetAll(UserPagingInputDto PagingInputDto)
        {
            var query = userManager.Users.AsQueryable();

            if (PagingInputDto.Filter != null)
            {
                var filter = PagingInputDto.Filter;
                query = query
                    .Where(u=>u.UserName.Contains(filter)||
                    u.Email.Contains(filter)||
                    u.FullName.Contains(filter)||
                    u.PhoneNumber.Contains(filter));
            }

            var order = query.OrderBy(PagingInputDto.OrderByField + " " + PagingInputDto.OrderType);

            var page = order.Skip((PagingInputDto.PageNumber * PagingInputDto.PageSize) - PagingInputDto.PageSize).Take(PagingInputDto.PageSize);

            var total = await query.CountAsync();

            var list = MapperObject.Mapper
                .Map<IList<UserAllDto>>(await page.ToListAsync());

            var response = new PagingResultDto<UserAllDto>
            {
                Result = list,
                Total = total
            };

            return response;
        }
   

        public async Task<UserDto> GetById(string id)
        {
            var entity = await userManager.Users
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync(x => x.Id == id);
            var response = MapperObject.Mapper.Map<UserDto>(entity);

            return response;
        }
        public async Task<UserDto> GetByIdWithoutChildren(string id)
        {
            var entity = await userManager.Users
                .FirstOrDefaultAsync(x => x.Id == id);
            var response = MapperObject.Mapper.Map<UserDto>(entity);

            return response;
        }

        public async Task Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            user.IsDeleted = true;

            await userManager.UpdateAsync(user);
        }
        public async Task UndoDelete(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            user.IsDeleted = false;

            await userManager.UpdateAsync(user);
        }
        public async Task HardDelete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            await userManager.DeleteAsync(user);
        }

        public async Task<UserDto> Create(UserDto input)
        {
            var userExists = await userManager.GetUserByPhoneAsync(input.PhoneNumber);
            if (userExists != null)
                throw new AppException(ExceptionEnum.RecordAlreadyExist);
            
            var user = MapperObject.Mapper.Map<ApplicationUser>(input);

            unitOfWork.BeginTran();

            //saving user
            var result = await userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
                throw new AppException(ExceptionEnum.RecordCreationFailed);
            input.Id=user.Id;
            
            //saving userRoles
            var userRoles = MapperObject.Mapper.Map<List<IdentityUserRole<string>>>(input.UserRoles);
            foreach (var role in userRoles)
            {
                role.UserId = user.Id;
                if ((await roleManager.FindByIdAsync(role.RoleId)) == null)
                    throw new AppException(ExceptionEnum.RecordNotExist);
            }
            await userRole.AddRangeAsync(userRoles);

            await unitOfWork.CompleteAsync();
            unitOfWork.CommitTran();

            return input;
        }

        public async Task<UserUpdateDto> Update(UserUpdateDto input)
        {
            var entity = await userManager.Users.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null)
                throw new AppException(ExceptionEnum.RecordNotExist);

            MapperObject.Mapper.Map(input, entity);
            
            unitOfWork.BeginTran();

            //saving user
            var result = await userManager.UpdateAsync(entity);
            if (!result.Succeeded)
                throw new AppException(ExceptionEnum.RecordUpdateFailed);

            //**saving userRoles

            //add new user roles
            var exsitedRolesIds = await userRole.GetUserRoleIdsByUserID(input.Id);
            if (input.UserRoles == null) input.UserRoles = new List<UserRoleDto>();
            var newAddedRoles = MapperObject.Mapper.Map<List<IdentityUserRole<string>>>(input.UserRoles.Where(x => !exsitedRolesIds.Contains(x.RoleId)));
            newAddedRoles.ForEach(x => x.UserId = input.Id);
            await userRole.AddRangeAsync(newAddedRoles);

            //delete removed roles
            var rolesIds = input.UserRoles.Select(x => x.RoleId).ToArray();
            var removedRoles = await userRole.GetRemovedUserRoleIdsByUserID(input.Id, rolesIds);
            await userRole.DeleteAsync(removedRoles.AsEnumerable());

            await unitOfWork.CompleteAsync();
            unitOfWork.CommitTran();

            return input;
        }

        public async Task<UserUpdateDto> UpdateWithoutChildren(UserUpdateDto input)
        {
            var entity = await userManager.Users.FirstOrDefaultAsync(x => x.Id == input.Id);
            
            if (entity == null)
                throw new AppException(ExceptionEnum.RecordNotExist);

            MapperObject.Mapper.Map(input, entity);
            //saving user
            var result = await userManager.UpdateAsync(entity);
            if (!result.Succeeded)
                throw new AppException(ExceptionEnum.RecordUpdateFailed);

            await unitOfWork.CompleteAsync();
            return input;
        }

        public async Task ForgetPassword(string userId)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x =>  (!x.IsDeleted)&& x.Id.Equals(userId));
            if(user == null)
                throw new AppException(ExceptionEnum.RecordNotExist);

            var token =await userManager.GeneratePasswordResetTokenAsync(user);
            string url = appSettings.JWT.Audience + "/login/forgetpassword/" + HttpUtility.UrlEncode(user.Id) + "/" + HttpUtility.UrlEncode(token);
        }
        public async Task<bool> ConfirmForgetPassword(ForgetPasswordDto input)
        {
            var user = await userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == input.UserId);
             if (user == null)
                throw new AppException(ExceptionEnum.RecordNotExist);

            var result= await userManager.ResetPasswordAsync(user,input.Token,input.Password);
            return result.Succeeded;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            var user = await userManager.FindByIdAsync(globalInfo.UserId);

            if (user == null)
                throw new AppException(ExceptionEnum.RecordNotExist);

            if(!await userManager.CheckPasswordAsync(user, input.OldPassword))
                throw new AppException(ExceptionEnum.WrongCredentials);

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, token, input.NewPassword);
            if (!result.Succeeded)
                throw new AppException(ExceptionEnum.RecordUpdateFailed);

            return true;
        }

        public async Task<SessionDto> GetUserSession()
        {
            var (user, permissions) = await userManager.GetUserSession(globalInfo.UserId);

            var response = MapperObject.Mapper.Map<SessionDto>(user);

            response.Permissions = permissions;

            return response;
        }
        public async Task<UserAllDto[]> GetUsersPermission(string permission)
        {
            var users= await userManager.GetUsersPermission(permission);
            var response = MapperObject.Mapper.Map<UserAllDto[]>(users);
            return response;
        }        
        public async Task StopUser(string userId)
        {
            var entity = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (entity == null)
                throw new AppException(ExceptionEnum.RecordNotExist);

            entity.IsActive = false;
            await unitOfWork.CompleteAsync();

        }
    }
}
