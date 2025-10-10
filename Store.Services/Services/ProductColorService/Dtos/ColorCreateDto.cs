using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.ProductColorService.Dtos
{
    public class ColorCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string ColorName { get; set; }
    }
}
