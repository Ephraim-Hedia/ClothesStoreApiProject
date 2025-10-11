
namespace Store.Data.Entities.ProductEntities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public Subcategory? Subcategory { get; set; }
        public int? SubcategoryId { get; set; }
        public List<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
        public List<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
        public List<ProductColorJoin> ProductColorJoins { get; set; } = new();
        public List<ProductSizeJoin> ProductSizeJoins { get; set; } = new();


        public List<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
        public List<Image> Images { get; set; } = new List<Image>();
        public Discount? Discount { get; set; }
        public int? DiscountId { get; set; }

    }
}
