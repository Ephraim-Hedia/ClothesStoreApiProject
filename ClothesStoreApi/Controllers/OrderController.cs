using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities.OrderEntities;
using Store.Services.Services.OrderService;
using Store.Services.Services.OrderService.Dtos;
using System.Security.Claims;

namespace Store.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // 🟢 POST: api/order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "User email not found in token." });

            var result = await _orderService.CreateOrderAsync(userEmail, dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        // 🟢 GET: api/order/{orderId}
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "User email not found in token." });

            var result = await _orderService.GetOrderByIdAsync(orderId, userEmail);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }

        // 🟢 GET: api/order/myorders
        [HttpGet("myorders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "User email not found in token." });

            var result = await _orderService.GetUserOrdersAsync(userEmail);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }



        // ============================================ These New APIs
        // 🟠 PUT: api/order/{orderId}/delivery
        [HttpPut("{orderId}/delivery")]
        public async Task<IActionResult> UpdateDelivery(int orderId, [FromBody] UpdateDeliveryDto dto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "User email not found in token." });

            var result = await _orderService.UpdateDeliveryAsync(orderId, dto, userEmail);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        // 🟠 PUT: api/order/{orderId}/items/quantity
        [HttpPut("{orderId}/items/quantity")]
        public async Task<IActionResult> UpdateOrderItemsQuantity(int orderId, [FromBody] List<UpdateOrderItemDto> items)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "User email not found in token." });

            var result = await _orderService.UpdateOrderItemsQuantityAsync(orderId, items, userEmail);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        // 🟠 PUT: api/order/{orderId}/status
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatus newStatus)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        // 🟢 POST: api/order/{orderId}/items
        [HttpPost("{orderId}/items")]
        public async Task<IActionResult> AddItemToOrder(int orderId, [FromBody] AddOrderItemDto dto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "User email not found in token." });

            var result = await _orderService.AddItemToOrderAsync(orderId, dto, userEmail);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        // 🔴 DELETE: api/order/{orderId}/items/{itemId}
        [HttpDelete("{orderId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromOrder(int orderId, int itemId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "User email not found in token." });

            var result = await _orderService.RemoveItemFromOrderAsync(orderId, itemId, userEmail);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        // 🟠 PUT: api/order/{orderId}/address
        [HttpPut("{orderId}/address")]
        public async Task<IActionResult> UpdateShippingAddress(int orderId, [FromBody] UpdateShippingAddressDto dto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "User email not found in token." });

            var result = await _orderService.UpdateShippingAddressAsync(orderId, dto, userEmail);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        // 🔴 DELETE: api/order/{orderId}
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "User email not found in token." });

            var result = await _orderService.CancelOrderAsync(orderId, userEmail);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
