using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Market.Core.Entities.Authorization;

namespace Market.Infrastructure.DataConfiguration
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            //builder.HasData
            //(
            //    new Permission()
            //    {
            //        Id = 1,
            //        Name = "MotorInvoice",
            //        Desc = "MotorInvoice",
            //        CategoryName="test"
            //    }

            //);
        }
    }
}