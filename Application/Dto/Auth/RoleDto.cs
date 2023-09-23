using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market.Application.Dto
{
    public class RoleDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public bool? IsAdmin { get; set; }

        //public IList<UserRoleDto>? UserRoles { get; set; }

        public IList<RolePermissionDto>? RolePermissions { get; set; }
    }
}
