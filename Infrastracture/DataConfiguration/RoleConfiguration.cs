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
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("Roles");
            //builder.HasData
            //( 
            //    new ApplicationRole()
            //    {
            //        Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin",
            //        ConcurrencyStamp = "1", NormalizedName = "Admin" ,
            //        IsAdmin = true
            //    }

            //);
        }
    }
}
