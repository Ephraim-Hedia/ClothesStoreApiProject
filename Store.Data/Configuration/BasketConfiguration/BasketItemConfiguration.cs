using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.BasketEntities;
using System.Reflection.Emit;

namespace Store.Data.Configuration.BasketConfiguration
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.HasOne(bi => bi.Basket)
            .WithMany(b => b.BasketItems)
            .HasForeignKey(bi => bi.BasketId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Property(bi => bi.Price)
            .HasPrecision(18, 2);
        }
    }
}
