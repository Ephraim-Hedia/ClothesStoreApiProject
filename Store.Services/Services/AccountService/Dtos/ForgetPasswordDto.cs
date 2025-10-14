using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.AccountService.Dtos
{
    public class ForgetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
