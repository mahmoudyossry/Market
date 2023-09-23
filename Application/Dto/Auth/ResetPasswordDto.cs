using System.ComponentModel.DataAnnotations;

namespace Market.Application.Dto
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
    }
}
