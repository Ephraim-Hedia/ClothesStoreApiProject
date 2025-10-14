namespace Store.Services.Services.UserService.Dtos
{
    public class UserUpdateDto
    {
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }
    }
}
