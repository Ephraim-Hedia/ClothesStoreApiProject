namespace Store.Data.Entities.ProductEntities
{
    public class ProductColor : BaseEntity<int>
    {
        public string ColorName { get; set; }
        public List<ProductColorJoin> ProductColorJoins { get; set; } = new();
    }
}
