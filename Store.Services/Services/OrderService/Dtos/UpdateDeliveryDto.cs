using Store.Data.Entities.OrderEntities;

namespace Store.Services.Services.OrderService.Dtos
{
    public class UpdateDeliveryDto 
    {
        public DeliveryStatus? Status { get; set; }
        public string? CourierName { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
    }
}
