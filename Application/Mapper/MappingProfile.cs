using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Market.Application.Dto;
using Market.Core.Entities;
using Market.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Core.Entities.Authorization;
using Market.Core.Global;
using Market.Core.Dto;
using Market.Core.IDto;

namespace Market.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //****GLOBAL WORNNIN FOR NAVIGATION PROPERTIES****
            //1- SHOULD NOT ACCESS ANY NAVIGATION PROPERTY WHEN MAPPING FROM DTO TO ENTITY,
            //   OTHERWISE IT WILL CREATE NEW RECORD FOR NAVIGATION ENTITY IN SAVING
            //2- SHOULD NOT USE .ReverseMap() BUT ONLY FOR SIMPLE ENTITY WITHOUT NAVIGATION PROPERTIES FOR THE SAME REASON
            //3- 

            //identity user

            CreateMap<UserDto, ApplicationUser>()
                .ForMember(m => m.UserRoles, op => op.Ignore())
                .ForMember(m => m.Id, op => op.Ignore());

            CreateMap<ApplicationUser, UserDto>();
            CreateMap<ApplicationUser, UserAllDto>();
                
            CreateMap<UserUpdateDto, ApplicationUser>()
                .ForMember(m => m.UserRoles, op => op.Ignore())
                .ForMember(m => m.Id, op => op.Ignore());

            CreateMap<ApplicationUser, UserUpdateDto>();

            //identity userRoles
            CreateMap<IdentityUserRole<string>, UserRoleDto>().ReverseMap();

            CreateMap<ApplicationRole, UserRoleDto>()
                .ForMember(m => m.RoleId, op => op.MapFrom(mp => mp.Id))
                .ForMember(m => m.RoleName, op => op.MapFrom(mp => mp.Name))
                .ReverseMap();

            //identity role
            CreateMap<RoleDto, ApplicationRole>()
                .ForMember(m => m.Id, op => op.Ignore())
                .ForMember(m => m.RolePermissions, op => op.Ignore())
                ;
            CreateMap<ApplicationRole, RoleDto>();

            CreateMap<RolePermissionDto, RolePermission>();
            CreateMap<RolePermission, RolePermissionDto>();

          
            CreateMap<Permission, PermissionDto>().ReverseMap();
            //SessionDto
            CreateMap<ApplicationUser, SessionDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest=>dest.IsAdmin,opt=>opt.MapFrom(src=>src.UserRoles.Any(r=>r.IsAdmin==true)));

            CreateMap<Product, ProductDto>().ReverseMap();
            
        }
    }
}
