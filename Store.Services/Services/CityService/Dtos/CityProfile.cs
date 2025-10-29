using AutoMapper;
using Store.Data.Entities.IdentityEntities;

namespace Store.Services.Services.CityService.Dtos
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CityCreateDto, City>().ReverseMap();
            CreateMap<CityResultDto, City>().ReverseMap();
            CreateMap<CityDropDownListResultDto, City>().ReverseMap();


        }
    }
}
