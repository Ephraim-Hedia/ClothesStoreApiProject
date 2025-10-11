using AutoMapper;
using Store.Data.Entities.ProductEntities;

namespace Store.Services.Services.ProductSizeService.Dtos
{
    public class SizeProfile : Profile
    {
        public SizeProfile()
        {
            CreateMap<ProductSize , SizeCreateDto>().ReverseMap();
            CreateMap<ProductSize, SizeResultDto>().ReverseMap();

        }
    }
}
