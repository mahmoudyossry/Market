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
using Market.Core.IDto;
using Market.Core.Entities;
using Market.Core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Market.Application.Services
{
    public class ProductService : BaseService<Product, ProductDto, ProductDto>, IProductService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductService(
             IUnitOfWork unitOfWork
            ):base(unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

    }
}
