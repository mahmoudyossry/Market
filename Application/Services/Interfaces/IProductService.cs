using Microsoft.AspNetCore.Identity;
using Market.Application.Dto;
using Market.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Core.IDto;

namespace Market.Application.Services.Interfaces
{
    public interface IProductService : IService<Product,ProductDto,ProductDto>
    {
    }
}
