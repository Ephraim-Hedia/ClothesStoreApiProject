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


        
        // Method to update delivery status
        public void UpdateStatus(DeliveryStatus newStatus)
        {
            // Prevent invalid transitions
            if (Status == DeliveryStatus.Delivered || Status == DeliveryStatus.Canceled)
                throw new InvalidOperationException("Cannot update delivery after it's completed or canceled.");

            Status = newStatus;
        }

        // Method to assign courier info later (optional)
        public void AssignCourier(string courierName, string trackingNumber)
        {
            CourierName = courierName;
            TrackingNumber = trackingNumber;
        }

        public void AddNote(string note)
        {
            Notes = note;
        }
    }

}
