using AutoMapper;
using Store.Data.Entities.ProductEntities;
using Store.Services.Services.ProductColorService.Dtos;
using Store.Services.Services.ProductSizeService.Dtos;


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
                .ForMember(dest => dest.ProductDiscount, opt => opt.MapFrom(src => src.Discount != null ? (decimal?)src.Discount.Percentage : null))
                .ForMember(dest => dest.CategoryDiscount, opt => opt.MapFrom(src => src.Category.Discount != null ? (decimal?)src.Category.Discount.Percentage : null))
                .ForMember(dest => dest.SubcategoryDiscount, opt => opt.MapFrom(src => src.Subcategory.Discount != null ? (decimal?)src.Subcategory.Discount.Percentage : null))

                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()))
                .ForMember(dest => dest.PriceBeforeDiscount, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.PriceAfterDiscount, opt => opt.MapFrom(src => src.GetPriceAfterBestDiscount()))
                // Map Colors
                .ForMember(dest => dest.ProductColors, opt => opt.MapFrom(
                    src => src.ProductColorJoins != null
                        ? src.ProductColorJoins
                            .Where(pc => pc.ProductColor != null)
                            .Select(pc => new ColorResultDto
                            {
                                Id = pc.ProductColor.Id,
                                ColorName = pc.ProductColor.ColorName
                            }).ToList()
                        : new List<ColorResultDto>()
                ))

                // Map Sizes
                .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(
                    src => src.ProductSizeJoins != null
                        ? src.ProductSizeJoins
                            .Where(ps => ps.ProductSize != null)
                            .Select(ps => new SizeResultDto
                            {
                                Id = ps.ProductSize.Id,
                                Name = ps.ProductSize.Name
                            }).ToList()
                        : new List<SizeResultDto>()
                ));

                // Map ProductCreateDto → Product
                CreateMap<ProductCreateDto, Product>()
                    .ForMember(dest => dest.Images, opt => opt.Ignore());
                // Map ProductUpdateDto → Product
                CreateMap<ProductUpdateDto, Product>()
                    .ForMember(dest => dest.Images, opt => opt.Ignore());

        }
    }
}
