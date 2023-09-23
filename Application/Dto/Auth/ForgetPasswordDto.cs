using System.ComponentModel.DataAnnotations;

namespace Market.Application.Dto
{
    public class ForgetPasswordDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
