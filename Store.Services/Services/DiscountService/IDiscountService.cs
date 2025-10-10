using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.DiscountService.Dtos;

namespace Store.Services.Services.DiscountService
{
    public interface IDiscountService
    {
        Task<CommonResponse<DiscountResultDto>> AddDiscountAsync(DiscountCreateDto dto);
        Task<CommonResponse<DiscountResultDto>> GetDiscountByIdAsync(int discountId);
        Task<CommonResponse<IReadOnlyList<DiscountResultDto>>> GetAllDiscountAsync();
        Task<CommonResponse<bool>> DeleteDiscountAsync(int discountId);
        Task<CommonResponse<DiscountResultDto>> UpdateDiscountAsync(int discountId, DiscountUpdateDto dto);
    }
}
