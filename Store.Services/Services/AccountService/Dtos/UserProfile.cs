using AutoMapper;

namespace Store.Services.Services.AccountService.Dtos
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, RegisterDto>();
            CreateMap<UserDto, LoginDto>();

        }
    }
}
