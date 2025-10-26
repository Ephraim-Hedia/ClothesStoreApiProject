using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.OrderEntities;

namespace Store.Data.Configuration.OrderConfiguration
{
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.ShippingAddress)
                   .WithMany()
                   .HasForeignKey(d => d.ShippingAddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(d => d.DeliveryPrice)
                   .HasColumnType("decimal(18,2)");

            builder.Property(d => d.Status)
                   .HasConversion<string>();

            builder.Property(d => d.CourierName)
                   .HasMaxLength(100);
        }
    }
}
