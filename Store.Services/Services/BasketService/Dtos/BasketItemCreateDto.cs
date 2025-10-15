namespace Store.Services.Services.BasketService.Dtos
{
    public class BasketItemCreateDto
    {
        public int ProductId { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }
        public int Quantity { get; set; }
    }
}
