namespace Store.Services.Services.UserService.Dtos
{
    public class UserCreateDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        // Optional contact info
        public string PhoneNumber { get; set; }

        // Optional address
        // Address Info

        public string? Street { get; set; }
        public int? CityId { get; set; }
    }
}
