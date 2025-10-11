

namespace Store.Data.Entities.ProductEntities
{
    public class ProductSizeJoin
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int ProductSizeId { get; set; }
        public ProductSize ProductSize { get; set; } = null!;
    }
}
