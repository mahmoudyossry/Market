using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market.Application.Dto
{
    public class UserUpdateDto
    {
        public string? Id { get; set; }
        public string Email { get; set; }
        [MaxLength(250, ErrorMessage = "Name cannot be more than 250 charachters")]
        public string? FullName { get; set; }
        public long StateId { get; set; }
        public long AreaId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string? FaceBookLink { get; set; }
        public string? ImgProfilePath { get; set; }
        public IList<UserRoleDto>? UserRoles { get; set; }
    }
}
