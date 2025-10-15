using AutoMapper;
using Store.Data.Entities.BasketEntities;

namespace Store.Services.Services.BasketService.Dtos
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<Basket, BasketResultDto>()
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()));
            CreateMap<BasketItem, BasketItemResultDto>().ReverseMap();
        }
    }
}
