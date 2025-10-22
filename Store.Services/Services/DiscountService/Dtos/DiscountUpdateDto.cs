namespace Store.Services.Services.DiscountService.Dtos
{
    public class DiscountUpdateDto
    {
        public string? Name { get; set; }
        public decimal? Percentage { get; set; }
        public List<int>? CategoryIds { get; set; }

    }
}
