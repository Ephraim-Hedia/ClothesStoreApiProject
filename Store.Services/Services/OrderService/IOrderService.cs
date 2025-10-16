using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.OrderService.Dtos;

namespace Store.Services.Services.OrderService
{
    public interface IOrderService
    {
        Task<CommonResponse<OrderResultDto>> CreateOrderAsync(string userId, OrderCreateDto dto);
        Task<CommonResponse<List<OrderResultDto>>> GetUserOrdersAsync(string userEmail);
        Task<CommonResponse<OrderResultDto>> GetOrderByIdAsync(int orderId, string userEmail);
    }
}
