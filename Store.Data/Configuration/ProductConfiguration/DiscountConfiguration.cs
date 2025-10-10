using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities;

namespace Store.Data.Configuration.ProductConfiguration
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> entity)
        {
            entity.Property(d => d.Percentage)
                .IsRequired()
                .HasPrecision(5, 2); // up to 999.99%
            entity.Property<string>(d => d.Name)
                .IsRequired();
            // No navigation back → just lightweight
        }
    }
}
