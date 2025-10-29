using Store.Data.Entities.IdentityEntities;

namespace Store.Data.Entities.OrderEntities
{
    public class ShippingAddress : BaseEntity<int>
    {
        public string Street { get; set; }
        public string? BuildingNumber { get; set; }
        public string? FloorNumber { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? Landmark { get; set; } // Optional: to help couriers (e.g. “near the hospital”)

        // More precise breakdown of location
        public string? District { get; set; } // Optional local area within city
        public int CityId { get; set; } 
        public City City { get; set; }

        // optional for historical tracing
        public string? RecipientName { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
