using Microsoft.AspNetCore.Identity;
using Market.Application.Dto;
using Market.Application.Dto;
using Market.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Services.Interfaces
{
    public interface IAuthenticateService
    {
        Task<object> Login(LoginDto model);
    }
}
