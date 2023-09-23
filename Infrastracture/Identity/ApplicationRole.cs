using Microsoft.AspNetCore.Identity;
using Market.Core.Entities;
using Market.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole, IFullAuditEntity<string>
    {

        public string? CreateUser { get; set; }
        public string CreateUserName { get; set; }
        public string? UpdateUser { get; set; }
        public string UpdateUserName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        public string DeleteUserId { get; set; }
        public bool? IsAdmin { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
