using AutoMapper;
using Store.Data.Entities.ProductEntities;

namespace Store.Services.Services.CategoriesService.Dtos
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryCreateDto, Category>().ReverseMap();

            CreateMap<Category, CategoryResultDto>()
               .ForMember(dest => dest.Discount, opt =>
                    opt.MapFrom(src => src.Discount != null ? src.Discount : null))
               .ForMember(dest => dest.Subcategories, opt => 
                    opt.MapFrom(src => src.Subcategories != null ? src.Subcategories : null))
               .ReverseMap();

            CreateMap<Category_DiscountResultDto, Discount>().ReverseMap();
            CreateMap<Category_SubcategoryResultDto, Subcategory>().ReverseMap();

        }
    }
}
