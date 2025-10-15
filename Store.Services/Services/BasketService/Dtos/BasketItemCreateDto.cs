using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.BasketService.Dtos
{
    public class BasketItemCreateDto
    {
        [Required] 
        public int ProductId { get; set; }
        [Required]
        public int ColorId { get; set; }
        [Required]
        public int SizeId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
