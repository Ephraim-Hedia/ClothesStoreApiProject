using Store.Data.Entities.OrderEntities;

namespace Store.Data.Entities.IdentityEntities
{
    public class City : BaseEntity<int>
    {
        public string Name { get; set; }
        // Delivery cost for orders in this city
        public decimal DeliveryCost { get; set; }

        // Optional estimated delivery time in days
        public int EstimatedDeliveryDays { get; set; }

        // Navigation
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();
    }
}
