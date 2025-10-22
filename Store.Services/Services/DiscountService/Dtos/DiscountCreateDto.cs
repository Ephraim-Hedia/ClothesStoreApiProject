using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.DiscountService.Dtos
{
    public class DiscountCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Percentage { get; set; }

        public List<int>? CategoryIds { get; set; }
        public List<int>? SubcategoryIds { get; set; }
        public List<int>? ProductsIds { get; set; }


    }
}
