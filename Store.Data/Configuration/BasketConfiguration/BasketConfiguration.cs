using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.BasketEntities;
using System.Reflection.Emit;

namespace Store.Data.Configuration.BasketConfiguration
{
    public class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.Property(b => b.ShippingPrice)
                    .HasPrecision(18, 2);
        }
    }
}
