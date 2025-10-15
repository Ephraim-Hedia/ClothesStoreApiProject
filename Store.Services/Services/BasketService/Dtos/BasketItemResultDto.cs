namespace Store.Services.Services.BasketService.Dtos
{
    public class BasketItemResultDto
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
