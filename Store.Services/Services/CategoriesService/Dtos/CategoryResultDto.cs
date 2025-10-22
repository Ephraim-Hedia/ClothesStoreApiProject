using Store.Data.Entities.ProductEntities;

namespace Store.Services.Services.CategoriesService.Dtos
{
    public class CategoryResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Category_SubcategoryResultDto>? Subcategories { get; set; }
        public Category_DiscountResultDto? Discount { get; set; } = null;
    }
}
