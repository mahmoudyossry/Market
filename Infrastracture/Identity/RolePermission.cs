using Market.Core.Entities.Authorization;
using Market.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Identity
{
    // This partial class to add the navigation prop ApplicationRole
    // because ApplicationRole is defined in Infrastructure.Identity and cannot be used in Core layer
    public class RolePermission : Entity //: Market.Core.Entities.Authorization.RolePermission
    {
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public ApplicationRole Role { get; set; }
        public long PermissionId { get; set; }

        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; }
        public string PermissionName { get; set; }
    }
}
