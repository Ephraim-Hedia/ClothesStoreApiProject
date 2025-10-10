using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities;

namespace Store.Data.Configuration.ProductConfiguration
{
    public class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
    {
        public void Configure(EntityTypeBuilder<ProductStock> entity)
        {
            // Composite Key
            entity.HasKey(ps => new { ps.ProductId, ps.ProductColorId, ps.ProductSizeId });

            entity.Property(ps => ps.Quantity)
                .IsRequired();

            entity.HasOne(ps => ps.Product)
                .WithMany(p => p.ProductStocks)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ps => ps.ProductColor)
                .WithMany()
                .HasForeignKey(ps => ps.ProductColorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ps => ps.ProductSize)
                .WithMany()
                .HasForeignKey(ps => ps.ProductSizeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
