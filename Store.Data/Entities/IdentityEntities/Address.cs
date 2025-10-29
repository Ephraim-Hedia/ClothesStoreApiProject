using System.ComponentModel.DataAnnotations;

namespace Store.Data.Entities.IdentityEntities
{
    public class Address : BaseEntity<int>
    {
        public string Street { get; set; }
        public string? BuildingNumber { get; set; }
        public string? FloorNumber { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? Landmark { get; set; } // Optional: to help couriers (e.g. “near the hospital”)

        // More precise breakdown of location
        public string? District { get; set; } // Optional local area within city

        // Foreign key to City
        public int CityId { get; set; } 
        public City City { get; set; }
        public string? RecipientName { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
