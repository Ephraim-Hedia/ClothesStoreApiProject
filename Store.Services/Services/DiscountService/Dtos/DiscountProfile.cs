using AutoMapper;
using Store.Data.Entities.ProductEntities;
using static Store.Services.Services.DiscountService.Dtos.DiscountResultDto;

namespace Store.Services.Services.DiscountService.Dtos
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount, DiscountResultDto>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Subcategories, opt => opt.MapFrom(src => src.Subcategories))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ReverseMap();

            CreateMap<Category, Discount_CategoryResultDto>().ReverseMap();
            CreateMap<Subcategory, Discount_SubcategoryResultDto>().ReverseMap();
            CreateMap<Product, Discount_ProductResultDto>().ReverseMap();

            CreateMap<Discount, DiscountCreateDto>().ReverseMap();

        }
    }
}
