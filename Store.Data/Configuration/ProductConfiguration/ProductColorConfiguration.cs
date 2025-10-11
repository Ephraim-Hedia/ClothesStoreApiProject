using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.ProductEntities;

namespace Store.Data.Configuration.ProductConfiguration
{
    public class ProductColorConfiguration : IEntityTypeConfiguration<ProductColor>
    {
        public void Configure(EntityTypeBuilder<ProductColor> entity)
        {
            entity.Property(pc => pc.ColorName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
