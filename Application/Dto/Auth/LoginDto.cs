using System.ComponentModel.DataAnnotations;

namespace Market.Application.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Phone is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
