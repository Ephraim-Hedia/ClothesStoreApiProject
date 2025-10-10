using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.CategoriesService.Dtos;

namespace Store.Services.Services.CategoriesService
{
    public interface ICategoryService
    {
        Task<CommonResponse<CategoryResultDto>> CreateCategoryAsync(CategoryCreateDto dto);
        Task<CommonResponse<CategoryResultDto>> UpdateCategoryAsync(int categoryId, CategoryUpdateDto dto);

        Task<CommonResponse<CategoryResultDto>> GetCategoryByIdAsync(int categoryId);
        Task<CommonResponse<IReadOnlyList<CategoryResultDto>>> GetAllCategoriesAsync();
        Task<CommonResponse<bool>> DeleteCategoryAsync(int categoryId);

    }
}
