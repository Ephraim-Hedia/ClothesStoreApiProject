using AutoMapper;
using Store.Data.Entities.ProductEntities;

namespace Store.Services.Services.SubcategoryService.Dtos
{
    public class SubcategoryProfile : Profile
    {
        public SubcategoryProfile()
        {
            CreateMap<Subcategory , SubcategoryCreateDto>().ReverseMap();
            CreateMap<Subcategory, SubcategoryResultDto>()
                .ForMember(dest => dest.Discount , opt => opt.MapFrom(dist => dist.Discount))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(dist => dist.Category))
                .ReverseMap();
            
            CreateMap<Discount, Subcategory_DiscountResultDto>().ReverseMap();
            CreateMap<Category, Subcategory_CategoryResultDto>().ReverseMap();

        }
    }
}
