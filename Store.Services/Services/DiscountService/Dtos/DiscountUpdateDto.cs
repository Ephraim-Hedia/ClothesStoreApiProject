namespace Store.Services.Services.DiscountService.Dtos
{
    public class DiscountUpdateDto
    {
        public string? Name { get; set; }
        public decimal? Percentage { get; set; }
        public List<int>? CategoryIds { get; set; }
        public List<int>? SubcategoryIds { get; set; }
        public List<int>? ProductsIds { get; set; }


    }
}
