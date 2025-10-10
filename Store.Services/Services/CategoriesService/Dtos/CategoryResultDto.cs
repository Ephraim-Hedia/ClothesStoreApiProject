using Store.Data.Entities;

namespace Store.Services.Services.CategoriesService.Dtos
{
    public class CategoryResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Subcategory> Subcategories { get; set; }
        public Discount? Discount { get; set; }
        public int? DiscountId { get; set; }
    }
}
