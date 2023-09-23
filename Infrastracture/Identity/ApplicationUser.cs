using Microsoft.AspNetCore.Identity;
using Market.Core.Entities;
using Market.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(250, ErrorMessage = "Name cannot be more than 250 charachters")]
        public string?FullName { get; set; }
        public int Type { get; set; }//manager -staff -player
        public string ?ImgProfilePath { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<ApplicationRole> UserRoles { get; set; }

    }
}
