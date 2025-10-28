namespace Store.Data.Entities.OrderEntities
{
    public enum OrderStatus
    {
        pending,
        canceled,
        delivered,
        completed,
        failed
    }
    public class Order : BaseEntity<int>
    {
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        // One-to-one with Delivery
        public int? DeliveryId { get; set; }        // ✅ FK here
        public Delivery DeliveryMethod { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.pending;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal Subtotal { get; set; }
        public int? BasketId { get; set; }
        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.DeliveryPrice;
        }
    }
}
