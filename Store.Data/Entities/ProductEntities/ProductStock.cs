namespace Store.Data.Entities.ProductEntities
{
    public class ProductStock 
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public ProductColor ProductColor { get; set; }
        public int ProductColorId { get; set; }
        public ProductSize ProductSize { get; set; }
        public int ProductSizeId { get; set; }
        public int Quantity { get; set; }
    }
}
