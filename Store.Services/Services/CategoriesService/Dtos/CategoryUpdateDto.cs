using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.CategoriesService.Dtos
{
    public class CategoryUpdateDto
    {
        [MaxLength(20)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
