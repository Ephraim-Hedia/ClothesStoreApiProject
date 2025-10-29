namespace Store.Services.Services.OrderService.Dtos
{
    public class OrderItemResultDto
    {
        public int Id { get; set; }
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
