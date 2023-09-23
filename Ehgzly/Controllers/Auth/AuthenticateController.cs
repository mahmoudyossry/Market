using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Market.Application.Dto;
using Market.Application.Filters;
using Market.Application.Middlewares;
using Market.Application.Services;
using Market.Application.Services.Interfaces;
using Market.Core.Global;
using Market.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService authenticateService;
        private readonly IUserService userService;

        public AuthenticateController(
            IAuthenticateService authenticateService,
            IUserService userService
            )
        {
            this.authenticateService = authenticateService;
            this.userService = userService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto model)
        {
            return Ok (await authenticateService.Login(model));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        [AppAuthorize]
        public async Task<SessionDto> GetUserSession()
        {
            return await userService.GetUserSession();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        public async Task<bool> TaskConfirmForgetPassword([FromBody] ForgetPasswordDto input)
        {
            return await userService.ConfirmForgetPassword(input);
        }
    }
}
