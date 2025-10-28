using Store.Data.Entities.OrderEntities;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.OrderService.Dtos;

namespace Store.Services.Services.OrderService
{
    public interface IOrderService
    {
        Task<CommonResponse<OrderResultDto>> CreateOrderAsync(string userId, OrderCreateDto dto);
        Task<CommonResponse<List<OrderResultDto>>> GetUserOrdersAsync(string userEmail);
        Task<CommonResponse<OrderResultDto>> GetOrderByIdAsync(int orderId, string userEmail);

        Task<CommonResponse<bool>> CancelOrderAsync(int orderId, string userEmail);
        Task<CommonResponse<OrderResultDto>> UpdateDeliveryAsync(int orderId, UpdateDeliveryDto dto, string userEmail);
        Task<CommonResponse<OrderResultDto>> UpdateOrderItemsQuantityAsync(int orderId, List<UpdateOrderItemDto> items, string userEmail);
        Task<CommonResponse<OrderResultDto>> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);

        Task<CommonResponse<OrderResultDto>> AddItemToOrderAsync(int orderId, AddOrderItemDto dto, string userEmail);
        Task<CommonResponse<OrderResultDto>> RemoveItemFromOrderAsync(int orderId, int itemId, string userEmail);

    }
}
