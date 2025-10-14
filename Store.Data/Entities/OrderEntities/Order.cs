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
        public OrderStatus PaymentStatus { get; set; } = OrderStatus.pending;
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal GetTotal()
        {
            return Subtotal + Delivery.GetShippingPrice(ShippingAddress.City);
        }
        public string? BasketId { get; set; }
    }
}
