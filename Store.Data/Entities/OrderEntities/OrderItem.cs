namespace Store.Data.Entities.OrderEntities
{
    public class OrderItem : BaseEntity<int>
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductItemOrdered ItemOrdered { get; set; }
        public int OrderId { get; set; }
    }
}
