using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.BasketService.Dtos
{
    public class BasketItemDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductSize { get; set; }
        [Required]
        public string ProductColor { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be more than zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 50, ErrorMessage = "The Quantity must be withen Range from : 1 to 50")]
        public int Quantity { get; set; }

   
    }
}
