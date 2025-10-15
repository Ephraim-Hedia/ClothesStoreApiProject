namespace Store.Data.Entities.BasketEntities
{
    public class Basket : BaseEntity<int>
    {
        public string UserId { get; set; }
        public decimal ShippingPrice { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public decimal GetTotal() => BasketItems.Sum(i => i.Quantity * i.Price); 
    }
}
