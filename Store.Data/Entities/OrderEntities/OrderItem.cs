namespace Store.Data.Entities.OrderEntities
{
    public class OrderItem : BaseEntity<int>
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductOrdered ItemOrdered { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
