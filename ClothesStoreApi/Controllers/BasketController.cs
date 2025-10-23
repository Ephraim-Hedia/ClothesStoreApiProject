using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.AccountService.Dtos;
using Store.Services.Services.BasketService;
using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.CategoriesService.Dtos;
using System.Security.Claims;

namespace Store.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService; 
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetBasket()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Invalid or missing token." });

            var result = await _basketService.GetUserBasketAsync(userId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddItem( BasketItemCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Invalid or missing token." });

            var result = await _basketService.AddItemAsync(userId , dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        [Route("{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Invalid or missing token." });

            var result = await _basketService.RemoveItemAsync(userId, itemId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateQuantity(BasketUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Invalid or missing token." });

            var result = await _basketService.UpdateQuantityAsync(userId, dto.ItemId , dto.Quantity);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }

        [HttpDelete]
        [Route("clear")]
        public async Task<IActionResult> ClearBasket()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Invalid or missing token." });

            var result = await _basketService.ClearBasketAsync(userId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
