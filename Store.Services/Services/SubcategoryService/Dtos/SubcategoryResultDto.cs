namespace Store.Services.Services.SubcategoryService.Dtos
{
    public class SubcategoryResultDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int? DiscountId { get; set; }
    }
}
