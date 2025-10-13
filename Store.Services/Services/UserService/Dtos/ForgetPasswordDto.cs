using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.UserService.Dtos
{
    public class ForgetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
