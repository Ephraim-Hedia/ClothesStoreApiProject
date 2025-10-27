using Store.Data.Entities.OrderEntities;
using Store.Services.Services.DeliveryService.Dtos;

namespace Store.Services.Services.OrderService.Dtos
{
    public class OrderResultDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        //public ShippingAddressDto ShippingAddress { get; set; }
        public DeliveryDto Delivery { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public List<OrderItemResultDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; } 
    }
}
