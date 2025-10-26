using Store.Data.Entities.IdentityEntities;

namespace Store.Data.Entities.OrderEntities
{
    public class ShippingAddress : BaseEntity<int>
    {
        public string Street { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }

        // optional for historical tracing
        public string? RecipientName { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
