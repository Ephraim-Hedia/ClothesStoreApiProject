namespace Store.Data.Entities
{
    public class Image : BaseEntity<int>
    {
        public string Url { get; set; }
        public int Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
