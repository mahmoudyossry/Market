using Microsoft.AspNetCore.Identity;
using Market.Application.Dto;
using Market.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagingResultDto<UserAllDto>> GetAll(UserPagingInputDto pagingInputDto);
        Task<UserDto> GetById(string id);
        Task<UserDto> GetByIdWithoutChildren(string id);
        Task Delete(string id);
        Task UndoDelete(string id);
        Task HardDelete(string id);
        Task<UserDto> Create(UserDto input);
        Task<UserUpdateDto> Update(UserUpdateDto input);
        Task<UserUpdateDto> UpdateWithoutChildren(UserUpdateDto input);
        Task ForgetPassword(string input);
        Task<bool> ConfirmForgetPassword(ForgetPasswordDto input);
        Task<bool> ResetPassword(ResetPasswordDto input);
        Task<SessionDto> GetUserSession();
        Task<UserAllDto[]> GetUsersPermission(string permission);
        Task StopUser(string userId);
    }
}
