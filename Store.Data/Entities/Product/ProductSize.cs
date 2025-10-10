namespace Store.Data.Entities
{
    public class ProductSize : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
