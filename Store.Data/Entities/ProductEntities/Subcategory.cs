namespace Store.Data.Entities.ProductEntities
{
    public class Subcategory : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public int? DiscountId { get; set; }
        public Discount? Discount { get; set; }
    }
}
