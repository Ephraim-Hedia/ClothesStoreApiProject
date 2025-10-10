namespace Store.Data.Entities
{
    public class Subcategory : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public Discount? Discount { get; set; }
        public int? DiscountId { get; set; }
    }
}
