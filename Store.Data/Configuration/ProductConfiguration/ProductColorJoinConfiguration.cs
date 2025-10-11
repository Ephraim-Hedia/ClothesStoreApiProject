using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.ProductEntities;

namespace Store.Data.Configuration.ProductConfiguration
{
    public class ProductColorJoinConfiguration : IEntityTypeConfiguration<ProductColorJoin>
    {
        public void Configure(EntityTypeBuilder<ProductColorJoin> entity)
        {
            entity.ToTable("productColorJoinTable");

            entity.HasKey(j => new { j.ProductId, j.ProductColorId });

            entity.HasOne(j => j.Product)
                .WithMany(p => p.ProductColorJoins)
                .HasForeignKey(j => j.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(j => j.ProductColor)
                .WithMany(c => c.ProductColorJoins)
                .HasForeignKey(j => j.ProductColorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
