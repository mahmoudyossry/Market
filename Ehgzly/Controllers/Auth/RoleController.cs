using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Dto;
using Market.Application.Filters;
using Market.Application.Middlewares;
using Market.Application.Services.Interfaces;
using Market.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Market.API.Controllers
{
    
    [AppAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    //[InputValidationActionFilter]
    //[SwaggerHeader("ABC", "Encrypted User.Sid got from client", "abc123", true)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        public async Task<PagingResultDto<RoleDto>> GetAll([FromQuery] PagingInputDto pagingInputDto)
        {
            return await roleService.GetAll(pagingInputDto);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        [AppAuthorize(Permissions = "Role")]
        //[SwaggerHeader("ABC", "Encrypted User.Sid got from client", "abc123", true)]
        public async Task<RoleDto> Get(string id)
        {
            //throw new Exception("Test exception");
            return await roleService.GetById(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        [AppAuthorize(Permissions = "Role.Create")]
        public async Task<ActionResult<RoleDto>> Create([FromBody] RoleDto input)
        {
            return await roleService.Create(input);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        [AppAuthorize(Permissions = "Role.Update")]
        public async Task Update([FromBody] RoleDto input)
        {
            await roleService.Update(input);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        [AppAuthorize(Permissions = "Role.Delete")]
        public async Task Delete(string id)
        {
            await roleService.Delete(id);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        public async Task<IList<PermissionDto>> GetAllPermissions()
        {
            return await roleService.GetAllPermissions();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        public async Task<IList<PermissionTreeDto>> GetAllPermissionsTree()
        {
            return await roleService.GetAllPermissionsTree();
        }

    }
}
