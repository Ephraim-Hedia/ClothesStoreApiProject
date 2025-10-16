using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
