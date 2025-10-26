using Store.Data.Entities.IdentityEntities;
using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.OrderService.Dtos
{
    public class ShippingAddressDto
    {
        public string Street { get; set; }
        public CityResultDto City { get; set; }

        // optional for historical tracing
        public string? RecipientName { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class CityResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
