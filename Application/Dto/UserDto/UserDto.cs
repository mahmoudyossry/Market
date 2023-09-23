using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market.Application.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string userName { get; set; }
        public string Email { get; set; }
        [MaxLength(250, ErrorMessage = "Name cannot be more than 250 charachters")]
        public string? FullName { get; set; }
        public int Type { get; set; }//manager -staff -player
        public long? ClubId { get; set; }
        public long StateId { get; set; }
        public long AreaId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string? FaceBookLink { get; set; }
        public string? ImgProfilePath { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public ICollection<UserRoleDto> UserRoles { get; set; }

    }
}
