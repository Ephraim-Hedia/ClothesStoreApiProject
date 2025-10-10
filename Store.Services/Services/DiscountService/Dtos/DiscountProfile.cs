using AutoMapper;
using Store.Data.Entities;

namespace Store.Services.Services.DiscountService.Dtos
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount , DiscountResultDto>().ReverseMap();
            CreateMap<Discount, DiscountCreateDto>().ReverseMap();

        }
    }
}
