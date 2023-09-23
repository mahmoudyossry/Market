using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Dto
{
    public class UserAllDto
    {
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int Type { get; set; }//manager -staff -player
    }
}
