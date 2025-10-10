using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.ProductService.Dtos
{
    public class ProductUpdateDto
    {
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public int? DiscountId { get; set; }

        public List<int>? ProductColorIds { get; set; }
        public List<int>? ProductSizeIds { get; set; }
        public List<int>? ImageUrls { get; set; }
    }
}
