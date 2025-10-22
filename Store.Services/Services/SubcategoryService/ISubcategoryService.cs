using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.SubcategoryService.Dtos;

namespace Store.Services.Services.SubcategoryService
{
    public interface ISubcategoryService
    {
        Task<CommonResponse<SubcategoryResultDto>> AddSubcategoryAsync(SubcategoryCreateDto dto);
        Task<CommonResponse<SubcategoryResultDto>> GetSubcategoryByIdAsync(int subcategoryId);
        Task<CommonResponse<IReadOnlyList<SubcategoryResultDto>>> GetAllSubcategoryAsync();
        Task<CommonResponse<bool>> DeleteSubcategoryAsync(int subcategoryId);
        Task<CommonResponse<SubcategoryResultDto>> UpdateSubcategoryAsync(int subcategoryId, SubcategoryUpdateDto dto);

        Task<CommonResponse<SubcategoryResultDto>> UpdateSubcategoryWithNewDiscountAsync(int subcategoryId, int discountId);
        Task<CommonResponse<bool>> UpdateSubcategoriesWithNewDiscountAsync(List<int> subcategoryIds, int discountId ,bool useExistingTransaction);

    }
}
