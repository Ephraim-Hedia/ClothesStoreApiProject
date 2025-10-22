using AutoMapper;
using Store.Data.Entities.ProductEntities;

namespace Store.Services.Services.CategoriesService.Dtos
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryCreateDto, Category>().ReverseMap();
            CreateMap<CategoryResultDto , Category>()
                .ForMember(dest => dest.Discount , option=> option.MapFrom(src => src.Discount))
                .ForMember(dest => dest.Subcategories, option => option.MapFrom(src => src.Subcategories))

                .ReverseMap();
            CreateMap<Category_DiscountResultDto, Discount>().ReverseMap();
            CreateMap<Category_SubcategoryResultDto, Subcategory>().ReverseMap();

        }
    }
}
