namespace Store.Data.Entities.ProductEntities
{
    public class Discount : BaseEntity<int>
    {
        public string Name { get; set; }
        public decimal Percentage { get; set; }

        // Navigation Properties
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Subcategory>? Subcategories { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
 