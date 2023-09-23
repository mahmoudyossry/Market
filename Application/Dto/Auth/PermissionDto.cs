using Market.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Dto
{
    public class PermissionDto : EntityDto
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Desc { get; set; }
        public bool Show { get; set; }
    }
}
