using System.ComponentModel.DataAnnotations;

namespace Market.Application.Dto
{
    public class RolePermissionDto : EntityDto
    {
        public long PermissionId { get; set; }

        public string PermissionName { get; set; }
    }
}
