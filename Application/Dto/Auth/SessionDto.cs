using System.Collections.Generic;

namespace Market.Application.Dto
{
    public class SessionDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool? IsAdmin { get; set; }
        public string[] Permissions { get; set; }
    }
}
