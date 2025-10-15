namespace Store.Data.Entities.BasketEntities
{
    public class CustomerBasket : BaseEntity<string>
    {
        public decimal ShippingPrice { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
    }
}
