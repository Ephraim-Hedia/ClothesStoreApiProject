using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.ProductEntities;

namespace Store.Data.Configuration.ProductConfiguration
{
    public class ProductSizeConfiguration : IEntityTypeConfiguration<ProductSize>
    {
        public void Configure(EntityTypeBuilder<ProductSize> entity)
        {
            entity.Property(ps => ps.Name)
            .IsRequired()
            .HasMaxLength(50);

            entity.Property(ps => ps.Description)
                .HasMaxLength(200);
        }
    }
}
