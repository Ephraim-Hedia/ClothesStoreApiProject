using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.OrderService.Dtos
{
    public class OrderCreateDto
    {
        public int BasketId { get; set; }
        public ShippingAddressCreateDto ShippingAddress { get; set; }
    }
    public class ShippingAddressCreateDto
    {
        [Required]
        public string Street { get; set; }
        [Required] 
        public int CityId { get; set; }
        // optional for historical tracing
        public string? RecipientName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
