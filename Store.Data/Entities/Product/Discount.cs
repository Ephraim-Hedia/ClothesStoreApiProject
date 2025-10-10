namespace Store.Data.Entities
{
    public class Discount : BaseEntity<int>
    {

        public string Name { get; set; }
        public decimal Percentage { get; set; }
    }
}
