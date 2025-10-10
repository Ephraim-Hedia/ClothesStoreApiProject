using AutoMapper;
using Store.Data.Entities;

namespace Store.Services.Services.ProductColorService.Dtos
{
    public class ColorProfile : Profile
    {
        public ColorProfile()
        {
            CreateMap<ColorCreateDto, ProductColor>().ReverseMap();
            CreateMap<ColorResultDto, ProductColor>().ReverseMap();

        }
    }
}
