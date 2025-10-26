using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.IdentityEntities;


namespace Store.Data.Configuration.UserConfiguration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Street)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(a => a.City)
                   .WithMany(c => c.Addresses)
                   .HasForeignKey(a => a.CityId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.ApplicationUser)
                   .WithMany(u => u.Addresses)
                   .HasForeignKey(a => a.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
