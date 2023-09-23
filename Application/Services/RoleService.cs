using Market.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Core.Repositories.Base;
using Market.Core.Entities;
using Market.Core.UnitOfWork;
using Market.Application.Services;
using Market.Application.Dto;
using Market.Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Market.Application.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Market.Infrastructure.Identity;
using Market.Core.Global;
using Microsoft.EntityFrameworkCore;
using Market.Application.Mapper;
using Market.Infrastructure.Data;
using Market.Infrastructure.UnitOfWorks;
using Market.Core.Entities.Base;
using System.Linq.Dynamic.Core;

namespace Market.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRolePermissionRepository<RolePermission> rolePermissionRepository;
        private readonly GlobalInfo globalInfo;

        public RoleService(RoleManager<ApplicationRole> roleManager
            , IUnitOfWork unitOfWork
            , IRolePermissionRepository<RolePermission> rolePermissionRepository
            ,GlobalInfo globalInfo
            )
        {
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
            this.rolePermissionRepository = rolePermissionRepository;
            this.globalInfo = globalInfo;
        }
        public virtual async Task<PagingResultDto<RoleDto>> GetAll(PagingInputDto PagingInputDto)
        {
            var query = roleManager.Roles.AsQueryable();

            if (PagingInputDto.Filter != null)
            {
                var filter = PagingInputDto.Filter;
                query = query.Where(u => u.Name.Contains(filter));
            }
         
            var order = query.OrderBy(PagingInputDto.OrderByField + " " + PagingInputDto.OrderType);

            var page = order.Skip((PagingInputDto.PageNumber * PagingInputDto.PageSize) - PagingInputDto.PageSize).Take(PagingInputDto.PageSize);

            var total = await query.CountAsync();

            var list = Market.Application.Mapper.MapperObject.Mapper
                .Map<IList<RoleDto>>(await page.ToListAsync());

            var response = new PagingResultDto<RoleDto>
            {
                Result = list,
                Total = total
            };

            return response;
        }

        public async Task<RoleDto> GetById(string id)
        {
            var entity = await roleManager.Roles.Include(x => x.RolePermissions).ThenInclude(x=>x.Permission).FirstOrDefaultAsync(x => x.Id == id);
            //var entity = await roleManager.Roles.Include(x => x.RolePermissions).FirstOrDefaultAsync(x => x.Id == id);
            var response = MapperObject.Mapper.Map<RoleDto>(entity);

            return response;
        }

        public async Task Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            role.IsDeleted = true;

            await roleManager.UpdateAsync(role);
        }

        public async Task<RoleDto> Create(RoleDto input)
        {
            var roleExists = await roleManager.FindByNameAsync(input.Name);
            if (roleExists != null)
                throw new AppException(ExceptionEnum.RecordAlreadyExist);
            unitOfWork.BeginTran();

            var entity = MapperObject.Mapper.Map<ApplicationRole>(input);
            var result = await roleManager.CreateAsync(entity);

            var newAddedRolePermissions = MapperObject.Mapper.Map<List<RolePermission>>(input.RolePermissions);
            newAddedRolePermissions.ForEach(x => x.RoleId = entity.Id);
            await rolePermissionRepository.AddRangeAsync(newAddedRolePermissions);

            await unitOfWork.CompleteAsync();
            unitOfWork.CommitTran();

            if (!result.Succeeded)
                throw new AppException(ExceptionEnum.RecordCreationFailed);

            return input;
        }

        public async Task<RoleDto> Update(RoleDto input)
        {
            //var entity = await roleManager.Roles.Include(x => x.RolePermissions).FirstOrDefaultAsync(x => x.Id == input.Id);
            var entity = await roleManager.Roles.Include(x => x.RolePermissions).FirstOrDefaultAsync(x => x.Id == input.Id);
            if (entity == null)
                throw new AppException(ExceptionEnum.RecordNotExist);


            MapperObject.Mapper.Map(input, entity, typeof(RoleDto), typeof(ApplicationRole));

            unitOfWork.BeginTran();

            var result = await roleManager.UpdateAsync(entity);
            if (!result.Succeeded)
                throw new AppException(ExceptionEnum.RecordUpdateFailed);

            //**saving userRoles

            //add new permissions
            var exsitedRolePermissionsIds = await rolePermissionRepository.GetRolePermissionsIdsByRoleID(input.Id);
            var newAddedRolePermissions = MapperObject.Mapper.Map<List<RolePermission>>(input.RolePermissions.Where(x => !exsitedRolePermissionsIds.Contains(x.PermissionId.ToString())));
            newAddedRolePermissions.ForEach(x => x.RoleId = input.Id);
            await rolePermissionRepository.AddRangeAsync(newAddedRolePermissions);

            //delete removed permissions
            var permissionIds = input.RolePermissions.Select(x => x.PermissionId.ToString()).ToArray();
            var removedRoles = await rolePermissionRepository.GetRemovedRolePermissionsIdsByRoleID(input.Id, permissionIds);
            await rolePermissionRepository.DeleteAsync(removedRoles.AsEnumerable());

            //**

            await unitOfWork.CompleteAsync();
            unitOfWork.CommitTran();

            return input;
        }

        public async Task<IList<PermissionDto>> GetAllPermissions()
        {
            IList<PermissionDto> response;

            var currTenantPermissions = await unitOfWork.Permission.GetAllAsList();
            response = MapperObject.Mapper.Map<IList<PermissionDto>>(currTenantPermissions);
            return response;
        }
        
        public async Task<IList<PermissionTreeDto>> GetAllPermissionsTree()
        {
            var permissionsList = await GetAllPermissions();
            List<PermissionTreeDto> response = new List<PermissionTreeDto>();
            var perantList=permissionsList.Where(r=>!r.Name.Contains(".")&&r.Show).ToList();
            var childList=permissionsList.Where(r=>r.Name.Contains(".")).ToList();
            
            foreach (var permission in perantList)
            {
                if (!response.Any(x => x.Label == permission.CategoryName))
                {
                    PermissionTreeDto permissionRoot = new PermissionTreeDto() { key = 0, Label = permission.CategoryName, Children = new List<PermissionTreeDto>() };
                    response.Add(permissionRoot);
                }

                var permissionNode=response.FirstOrDefault(x=>x.Label==permission.CategoryName);
                permissionNode.Children.Add(new PermissionTreeDto() { key=permission.Id,Label=permission.Name,Children = new List<PermissionTreeDto>() } );

                foreach (var child in childList)
                {
                    if (child.Name.StartsWith(permission.Name + "."))
                    {
                        permissionNode.Children.FirstOrDefault(x=>x.Label==permission.Name).Children
                            .Add(new PermissionTreeDto() { key = child.Id, Label = child.Name, Children = new List<PermissionTreeDto>() });
                    }
                }

            }
            return response;
        }
    }
}
