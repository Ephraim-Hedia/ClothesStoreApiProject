namespace Store.Data.Entities.ProductEntities
{
    public class ProductColorJoin
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int ProductColorId { get; set; }
        public ProductColor ProductColor { get; set; } = null!;
    }
}
