using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using System.Reflection;
using System.Reflection.Emit;

namespace Store.Data.Context
{
    public class StoreClothesDbContext : IdentityDbContext
    {
        public StoreClothesDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Subcategory> Subcategories { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<ProductColor> ProductColors { get; set; }
        DbSet<ProductSize>  ProductSizes { get; set; }
        DbSet<Discount> Discounts { get; set; }
        DbSet<ProductStock> ProductStocks { get; set; }
    }
}
