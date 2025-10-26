namespace Store.Data.Entities.OrderEntities
{
    public enum DeliveryStatus
    {
        Pending,
        InProgress,
        Delivered,
        Canceled
    }
    public class Delivery : BaseEntity<int>
    {
        
        // Delivery details
        public decimal DeliveryPrice { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public DeliveryStatus Status { get; set; }

        // Shipping info (snapshot of where to deliver)
        public int ShippingAddressId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        // Optional fields
        // Delivery company or courier info
        public string? CourierName { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
    }
}
