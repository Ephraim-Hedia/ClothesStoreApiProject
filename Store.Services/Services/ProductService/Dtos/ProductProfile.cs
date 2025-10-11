using AutoMapper;
using Store.Data.Entities.ProductEntities;


namespace Store.Services.Services.ProductService.Dtos
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            
                // Map Product → ProductResultDto
                CreateMap<Product, ProductResultDto>()
                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                    .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.Subcategory != null ? src.Subcategory.Name : null))
                    .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.Discount != null ? (decimal?)src.Discount.Percentage : null))
                    .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));

                // Map ProductCreateDto → Product
                CreateMap<ProductCreateDto, Product>()
                    .ForMember(dest => dest.Images, opt => opt.Ignore());

        }
    }
}
