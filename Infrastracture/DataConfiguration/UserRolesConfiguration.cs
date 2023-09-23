using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.DataConfiguration
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.ToTable("UserRoles");
            //builder.HasData
            //(
            //    new IdentityUserRole<string>()
            //    {
                    
            //        RoleId = "fab4fac1-c546-41de-aebc-a14da6895711",
            //        UserId = "b74ddd14-6340-4840-95c2-db12554843e5",
            //    }
            //);
        }
    }

}
