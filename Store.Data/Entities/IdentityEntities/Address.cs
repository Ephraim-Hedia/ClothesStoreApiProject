using System.ComponentModel.DataAnnotations;

namespace Store.Data.Entities.IdentityEntities
{
    public class Address : BaseEntity<int>
    {
        public string Street { get; set; }

        // Foreign key to City
        public int? CityId { get; set; }
        public City City { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
