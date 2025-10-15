namespace Store.Services.Services.BasketService.Dtos
{
    public class BasketItemResultDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
