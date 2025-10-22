using Store.Data.Entities.ProductEntities;

namespace Store.Services.Services.SubcategoryService.Dtos
{
    public class SubcategoryResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Subcategory_CategoryResultDto Category { get; set; }
        public Subcategory_DiscountResultDto? Discount { get; set; }
    }
}
