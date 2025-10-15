using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Data.Entities.BasketEntities;
using Store.Data.Entities.IdentityEntities;
using Store.Data.Entities.OrderEntities;
using Store.Data.Entities.ProductEntities;
using System.Reflection;

namespace Store.Data.Context
{
    public class StoreClothesDbContext : IdentityDbContext<ApplicationUser ,ApplicationRole, string>
    {
        public StoreClothesDbContext(DbContextOptions<StoreClothesDbContext> options) : base(options)
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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
    }
}
