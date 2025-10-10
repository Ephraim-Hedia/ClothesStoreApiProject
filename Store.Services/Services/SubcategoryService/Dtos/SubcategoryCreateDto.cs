using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.SubcategoryService.Dtos
{
    public class SubcategoryCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string? Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int? DiscountId { get; set; }

    }
}
