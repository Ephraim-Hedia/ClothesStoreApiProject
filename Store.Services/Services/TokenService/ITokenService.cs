using Store.Data.Entities.IdentityEntities;

namespace Store.Services.Services.TokenService
{
    public interface ITokenService
    {
        public string GenerateToken(ApplicationUser applicationUser);
    }
}
