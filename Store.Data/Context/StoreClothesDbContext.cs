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
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize>  ProductSizes { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<City> Cities { get; set; }



    }
}
