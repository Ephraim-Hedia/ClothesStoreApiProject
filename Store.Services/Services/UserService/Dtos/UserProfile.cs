using AutoMapper;
using Store.Data.Entities.IdentityEntities;
using Store.Services.Services.AddressService.Dtos;

namespace Store.Services.Services.UserService.Dtos
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserResultDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses)).ReverseMap();

            CreateMap<ApplicationUser, UserCreateDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();
            // Address mappings
            CreateMap<City, CityResultDto>();
            CreateMap<AddressCreateDto, Address>().ReverseMap();
            CreateMap<Address, AddressResultDto>().ReverseMap();
        }
    
    }
}
