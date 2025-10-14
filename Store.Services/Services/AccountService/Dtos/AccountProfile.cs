using AutoMapper;

namespace Store.Services.Services.AccountService.Dtos
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<UserDto, RegisterDto>();
            CreateMap<UserDto, LoginDto>();

        }
    }
}
