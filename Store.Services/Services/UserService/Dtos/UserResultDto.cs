using Store.Services.Services.AddressService.Dtos;

namespace Store.Services.Services.UserService.Dtos
{
    public class UserResultDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<AddressResultDto> Addresses { get; set; }
    }
}
