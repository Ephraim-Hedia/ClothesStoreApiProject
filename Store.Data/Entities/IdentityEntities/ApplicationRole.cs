using Microsoft.AspNetCore.Identity;

namespace Store.Data.Entities.IdentityEntities
{
    public class ApplicationRole : IdentityRole
    {
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
