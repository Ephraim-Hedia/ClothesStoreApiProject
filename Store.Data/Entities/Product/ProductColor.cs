namespace Store.Data.Entities
{
    public class ProductColor : BaseEntity<int>
    {
        public string ColorName { get; set; }
        public Product? Product { get; set; }
        public int? ProductId { get; set; }
    }
}
