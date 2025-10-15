using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.BasketService.Dtos;

namespace Store.Services.Services.BasketService
{
    public interface IBasketService
    {
        Task<CommonResponse<BasketResultDto>> GetUserBasketAsync(string userId);
        Task<CommonResponse<BasketResultDto>> AddItemAsync(string userId, BasketItemCreateDto dto);
        Task<CommonResponse<bool>> RemoveItemAsync(string userId, int productId);
        Task<CommonResponse<BasketResultDto>> UpdateQuantityAsync(string userId, int productId, int quantity);
        Task<CommonResponse<bool>> ClearBasketAsync(string userId);
    }
}
