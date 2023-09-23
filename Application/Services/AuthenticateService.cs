using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Market.Application.Dto;
using Market.Application.Services.Interfaces;
using Market.Core;
using Market.Core.Entities;
using Market.Core.Global;
using Market.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly ApplicationUserManager userManager;
        private readonly GlobalInfo globalInfo;
        private readonly AppSettingsConfiguration settings;

        public AuthenticateService(
            ApplicationUserManager userManager,
            GlobalInfo globalInfo,
            AppSettingsConfiguration settings)
        {
            this.userManager = userManager;
            this.globalInfo = globalInfo;
            this.settings = settings;
        }

        public async Task<object> Login(LoginDto model)
        {
            var user = await userManager.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber && x.IsActive);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                globalInfo.UserId = user.Id.ToString();
                globalInfo.UserName = user.UserName;

                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("userId",user.Id.ToString()),
                    new Claim("type",user.Type.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JWT.SecretKey));

                DateTime expires = DateTime.Now.AddMonths(6);

                var token = new JwtSecurityToken(
                    issuer: settings.JWT.Issuer,
                    audience: settings.JWT.Audience,
                    expires: expires,
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new TokenDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                };
            }

            throw new AppException(ExceptionEnum.WrongCredentials);
        }
    }

    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
