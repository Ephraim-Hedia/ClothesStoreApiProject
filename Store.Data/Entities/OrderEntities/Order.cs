namespace Store.Data.Entities.OrderEntities
{
    public enum OrderStatus
    {
        pending,
        Completed,
        failed
    }
    public class Order : BaseEntity<int>
    {
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public ShippingAddress ShippingAddress { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.pending;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal Subtotal { get; set; }
        public int? BasketId { get; set; }
        public decimal GetTotal()
        {
            return Subtotal + Delivery.GetShippingPrice(ShippingAddress.City);
        }
    }
}
