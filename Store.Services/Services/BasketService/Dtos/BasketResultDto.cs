namespace Store.Services.Services.BasketService.Dtos
{
    public class BasketResultDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<BasketItemResultDto> Items { get; set; }
        public decimal Total { get; set; } 
    }
}
