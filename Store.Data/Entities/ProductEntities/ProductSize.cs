namespace Store.Data.Entities.ProductEntities
{
    public class ProductSize : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<ProductSizeJoin> ProductSizeJoins { get; set; } = new();
    }
}
