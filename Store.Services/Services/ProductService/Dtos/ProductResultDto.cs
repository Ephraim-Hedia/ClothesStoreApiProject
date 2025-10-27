using Store.Services.Services.ProductColorService.Dtos;
using Store.Services.Services.ProductSizeService.Dtos;

namespace Store.Services.Services.ProductService.Dtos
{
    public class ProductResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PriceBeforeDiscount { get; set; }
        public decimal PriceAfterDiscount { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int? SubcategoryId { get; set; }
        public string? SubcategoryName { get; set; }

        public decimal? ProductDiscount { get; set; }
        public decimal? CategoryDiscount { get; set; }
        public decimal? SubcategoryDiscount { get; set; }


        // Related data references
        public List<ColorResultDto> ProductColors { get; set; }
        public List<SizeResultDto> ProductSizes { get; set; }
        public List<ImageResultDto> Images { get; set; }
    }
}
