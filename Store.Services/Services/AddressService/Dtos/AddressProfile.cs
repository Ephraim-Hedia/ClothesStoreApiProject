using AutoMapper;
using Store.Data.Entities.IdentityEntities;

namespace Store.Services.Services.AddressService.Dtos
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<City, CityResultDto>();
            CreateMap<AddressCreateDto, Address>().ReverseMap();
            CreateMap<AddressResultDto, Address>().ReverseMap();

        }
    }
}
