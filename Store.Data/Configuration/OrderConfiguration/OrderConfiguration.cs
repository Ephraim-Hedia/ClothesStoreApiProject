using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.OrderEntities;

namespace Store.Data.Configuration.OrderConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Delivery)
                   .WithOne(d => d.Order)
                   .HasForeignKey<Delivery>(d => d.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.Subtotal)
                   .HasColumnType("decimal(18,2)");

            builder.Property(o => o.BuyerEmail)
                   .IsRequired();

            builder.Property(o => o.OrderStatus)
                   .HasConversion<string>();
            builder.HasMany(o => o.OrderItems)
               .WithOne(oi => oi.Order)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
