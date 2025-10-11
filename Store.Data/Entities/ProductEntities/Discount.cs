namespace Store.Data.Entities.ProductEntities
{
    public class Discount : BaseEntity<int>
    {
        public string Name { get; set; }
        public decimal Percentage { get; set; }
    }
}
 