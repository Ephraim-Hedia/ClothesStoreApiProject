using Store.Data.Entities.ProductEntities;

namespace Store.Data.Entities.BasketEntities
{
    public class BasketItem : BaseEntity<int>
    {
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int SizeId { get; set; }
        public ProductSize Size { get; set; }

        public int ColorId { get; set; }
        public ProductColor Color { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } 
    }
}
