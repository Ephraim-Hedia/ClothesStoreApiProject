using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.AccountService.Dtos;
using Store.Services.Services.BasketService;
using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.CategoriesService.Dtos;

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
        [Route("{userId}")]
        public async Task<IActionResult> GetBasket(string userId)
        {
            var result = await _basketService.GetUserBasketAsync(userId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }

        [HttpPost]
        [Route("{userId}/add")]
        public async Task<IActionResult> AddItem(string userId, BasketItemCreateDto dto)
        {
            var result = await _basketService.AddItemAsync(userId , dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        [Route("{userId}/remove/{productId}")]
        public async Task<IActionResult> RemoveItem(string userId, int productId)
        {
            var result = await _basketService.RemoveItemAsync(userId, productId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }

        [HttpPut]
        [Route("{userId}/update/{productId}/{quantity}")]
        public async Task<IActionResult> UpdateQuantity(string userId, int productId, int quantity)
        {
            var result = await _basketService.UpdateQuantityAsync(userId, productId , quantity);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }

        [HttpDelete]
        [Route("{userId}/clear")]
        public async Task<IActionResult> ClearCard(string userId)
        {
            var result = await _basketService.ClearBasketAsync(userId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
