using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.ProductEntities;

namespace Store.Data.Configuration.ProductConfiguration
{
    internal class ProductSizeJoinConfiguration : IEntityTypeConfiguration<ProductSizeJoin>
    {
        public void Configure(EntityTypeBuilder<ProductSizeJoin> entity)
        {
            entity.ToTable("productSizeJoinTable");

            entity.HasKey(j => new { j.ProductId, j.ProductSizeId });

            entity.HasOne(j => j.Product)
                .WithMany(p => p.ProductSizeJoins)
                .HasForeignKey(j => j.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(j => j.ProductSize)
                .WithMany(s => s.ProductSizeJoins)
                .HasForeignKey(j => j.ProductSizeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
