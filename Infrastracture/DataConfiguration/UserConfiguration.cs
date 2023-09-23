using Microsoft.AspNetCore.Identity;
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
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;

        public UserConfiguration(IPasswordHasher<ApplicationUser> passwordHasher)
        {
            this.passwordHasher = passwordHasher;
        }

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            //ApplicationUser user = new ApplicationUser()
            //{
            //    Id = "b74ddd14-6340-4840-95c2-db12554843e5",
            //    UserName = "admin",
            //    NormalizedUserName = "ADMIN",
            //    Email = "admin@default.com",
            //    NormalizedEmail = "ADMIN@DEFAULT.COM",
            //    LockoutEnabled = false,
            //    PhoneNumber = "1234567890",
            //    //PasswordHash = passwordHasher.HashPassword(null, "neom123")
            //    PasswordHash = "AQAAAAEAACcQAAAAEG3KSpf5uwOA1PJzZMehh79IE98DUJ4hUXXzFJ2PBbHN2ebRdhNwad9yudpRhpiu9Q==",
            //    SecurityStamp = "86e94f96-8995-4181-88e6-693df7c2bcfd",
            //    ConcurrencyStamp = "023eeac9-72a3-4e9d-bc8e-29ae4e2fcb68",
                
            //};


            //builder.HasData
            //(
            //    user
            //);
        }
    }

}
