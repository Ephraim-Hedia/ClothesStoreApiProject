using Store.Data.Entities.OrderEntities;

namespace Store.Services.Services.OrderService.Dtos
{
    public class DeliveryDto
    {
        // Delivery details
        public decimal DeliveryPrice { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public DeliveryStatus Status { get; set; }

        // Shipping info (snapshot of where to deliver)
        public ShippingAddressDto ShippingAddress { get; set; }

        // Optional fields
        // Delivery company or courier info
        public string? CourierName { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
    }
}
