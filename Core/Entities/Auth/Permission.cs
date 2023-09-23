using Market.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Market.Core.Entities.Authorization
{
    public class Permission : Entity
    {
        [MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string CategoryName { get; set; }

        [MaxLength(250)]
        public string Desc { get; set; }
        public bool Show { get; set; }
    }
}
