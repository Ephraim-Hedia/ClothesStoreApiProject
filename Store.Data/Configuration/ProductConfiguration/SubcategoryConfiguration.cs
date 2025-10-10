using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities;

namespace Store.Data.Configuration.ProductConfiguration
{
    public class SubcategoryConfiguration : IEntityTypeConfiguration<Subcategory>
    {
        public void Configure(EntityTypeBuilder<Subcategory> entity)
        {
            entity.Property(sc => sc.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(sc => sc.Description)
                .HasMaxLength(500);

            // Optional Discount
            entity.HasOne(sc => sc.Discount)
                .WithMany()
                .HasForeignKey(sc => sc.DiscountId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
