using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.BasketService.Dtos;

namespace Store.Services.Services.BasketService
{
    public interface IBasketService
    {
        Task<CommonResponse<BasketResultDto>> GetUserBasketAsync(string? userId, string? fingerPrint);
        Task<CommonResponse<BasketResultDto>> AddItemAsync(string? userId, string? fingerPrint, BasketItemCreateDto dto);
        Task<CommonResponse<bool>> RemoveItemAsync(string? userId, string? fingerPrint, int itemId);
        Task<CommonResponse<BasketResultDto>> UpdateQuantityAsync(string? userId, string? fingerPrint, int itemId, int quantity);
        Task<CommonResponse<bool>> ClearBasketAsync(string? userId, string? fingerPrint);
    }
}
