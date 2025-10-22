using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.ProductEntities;
using System.Reflection.Emit;

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

            // Relationships
            entity.Property(d => d.Percentage)
                .IsRequired()
                .HasPrecision(5, 2); // up to 999.99%

            entity.Property(d => d.Name)
                .IsRequired();

            // ✅ Relationships — use the navigation properties
            entity.HasMany(d => d.Products)
              .WithOne(p => p.Discount)
              .HasForeignKey(p => p.DiscountId)
              .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(d => d.Categories)
                  .WithOne(c => c.Discount)
                  .HasForeignKey(c => c.DiscountId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(d => d.Subcategories)
                  .WithOne(s => s.Discount)
                  .HasForeignKey(s => s.DiscountId)
                  .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
