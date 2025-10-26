using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.OrderEntities;

namespace Store.Data.Configuration.OrderConfiguration
{
    public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
    {
        public void Configure(EntityTypeBuilder<ShippingAddress> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Street)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(s => s.City)
                   .WithMany(c => c.ShippingAddresses)
                   .HasForeignKey(s => s.CityId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
