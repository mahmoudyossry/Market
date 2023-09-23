using Microsoft.AspNetCore.Mvc;
using Market.Application.Dto;
using Market.Application.Filters;
using Market.Application.Services.Interfaces;


namespace Market.API.Controllers
{
   // [AppAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    //[InputValidationActionFilter]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        public async Task<PagingResultDto<UserAllDto>> GetAll([FromQuery] UserPagingInputDto pagingInputDto)
        {
            return await userService.GetAll(pagingInputDto);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        public async Task<UserAllDto[]> GetAllInPermission(string permission)
        {
            return await userService.GetUsersPermission(permission);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        //[AppAuthorize(Permissions = "User")]
        public async Task<UserDto> Get(string id)
        {
            //throw new Exception("Test exception");
            return await userService.GetById(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        //[AppAuthorize(Permissions = "User.Create")]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserDto input)
        {
            return await userService.Create(input);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        //[AppAuthorize(Permissions = "User.Update")]
        public async Task Update([FromBody] UserUpdateDto input)
        {
            await userService.Update(input);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        //[AppAuthorize(Permissions = "User.Delete")]
        public async Task Delete(string id)
        {
            await userService.Delete(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        public async Task<bool> ResetPassword([FromBody] ResetPasswordDto input)
        {
            return await userService.ResetPassword(input);
        }

       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        //[AppAuthorize(Permissions = "User.ForgetPassword")]
        public async Task ForgetPassword( string input)
        {
            await userService.ForgetPassword(input);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        //[AppAuthorize(Permissions = "User.Stop")]
        public async Task StopUser(string userId)
        {
            await userService.StopUser(userId);
        }
    }
}
