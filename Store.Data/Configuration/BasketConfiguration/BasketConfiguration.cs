using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.BasketEntities;

namespace Store.Data.Configuration.BasketConfiguration
{
    public class BasketConfiguration : IEntityTypeConfiguration<CustomerBasket>
    {
        public void Configure(EntityTypeBuilder<CustomerBasket> builder)
        {
            builder.OwnsMany(basket => basket.BasketItems, BasketItems =>
            {
                BasketItems.WithOwner();
            });
        }
    }
}
