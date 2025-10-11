using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.ProductEntities;

namespace Store.Data.Configuration.ProductConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {       
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Description)
                .HasMaxLength(500);

            // One-to-many: Category → Subcategories
            entity.HasMany(c => c.Subcategories)
                .WithOne(sc => sc.Category)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many: Category → Products
            entity.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional Discount
            entity.HasOne(c => c.Discount)
                .WithMany()
                .HasForeignKey(c => c.DiscountId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
