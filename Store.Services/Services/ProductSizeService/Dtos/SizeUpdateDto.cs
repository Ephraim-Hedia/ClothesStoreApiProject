using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.ProductSizeService.Dtos
{
    public class SizeUpdateDto
    {
        [MaxLength(20)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
