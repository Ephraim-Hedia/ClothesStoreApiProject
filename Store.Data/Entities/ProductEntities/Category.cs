namespace Store.Data.Entities.ProductEntities
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        public List<Product> Products { get; set; } 
        public int? DiscountId { get; set; } 
        public Discount? Discount { get; set; }
    }
}
