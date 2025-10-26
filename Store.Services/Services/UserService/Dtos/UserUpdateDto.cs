namespace Store.Services.Services.UserService.Dtos
{
    public class UserUpdateDto
    {
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }

        // Address Info
        public string? Street { get; set; }
        public int? CityId { get; set; }
    }
}
