using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities;

namespace Store.Data.Configuration.ProductConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.Description)
                .HasMaxLength(150);

            entity.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Subcategory)
                .WithMany(sc => sc.Products)
                .HasForeignKey(p => p.SubcategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(p => p.Discount)
                .WithMany()
                .HasForeignKey(p => p.DiscountId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
