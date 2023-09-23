using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Market.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.DataConfiguration
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {

            builder.ToTable("RolePermissions");
            //builder.HasData
            //(
            //    //Client
            //    new RolePermission()
            //    {
            //        Id = 1,
            //        PermissionId = 17,
            //        PermissionName = "Client",
            //        RoleId = "38DF70AC-AB12-43D4-B060-D96AE7B3EC74"
            //    }

            //);
        }
    }
}
