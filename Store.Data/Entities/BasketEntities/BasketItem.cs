namespace Store.Data.Entities.BasketEntities
{
    public class BasketItem : BaseEntity<int>
    {
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
