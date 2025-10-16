using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.OrderService.Dtos
{
    public class OrderCreateDto
    {
        public int BasketId { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
    }
}
