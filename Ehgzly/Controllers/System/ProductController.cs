using Microsoft.AspNetCore.Mvc;
using Market.Application.Dto;
using Market.Application.Filters;
using Market.Application.Services.Interfaces;
using Market.Core.Entities;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AppAuthorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;

        public ProductController(IProductService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        public async Task<PagingResultDto<ProductDto>> GetAll([FromQuery]PagingInputDto pagingInputDto)
        {
            return await service.GetAll(pagingInputDto);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        //[AppAuthorize(Permissions = "Product")]
        public async Task<ProductDto> Get(long id)
        {
            return await service.GetById(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        //[AppAuthorize(Permissions = "Product.Create")]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductDto input)
        {
            return await service.Create(input);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        //[AppAuthorize(Permissions = "Product.Update")]
        public async Task Update([FromBody] ProductDto input)
        {
            await service.Update(input);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]")]
        //[AppAuthorize(Permissions = "Product.Delete")]
        public async Task Delete(long id)
        {
            await service.Delete(id);
        }

    }
}
