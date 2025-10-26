using Microsoft.AspNetCore.Identity;

namespace Store.Data.Entities.IdentityEntities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Address>? Addresses { get; set; }
    }
}
