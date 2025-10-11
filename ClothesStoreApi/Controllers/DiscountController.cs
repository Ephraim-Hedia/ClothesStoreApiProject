using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.CategoriesService.Dtos;
using Store.Services.Services.DiscountService;
using Store.Services.Services.DiscountService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddDiscount(DiscountCreateDto dto)
        {
            var result = await _discountService.AddDiscountAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllDiscounts()
        {
            var result = await _discountService.GetAllDiscountAsync();
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet]
        [Route("{discountId}")]
        public async Task<IActionResult> GetDiscount(int discountId)
        {
            var result = await _discountService.GetDiscountByIdAsync(discountId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{discountId}")]
        public async Task<IActionResult> DeleteDiscount(int discountId)
        {
            var result = await _discountService.DeleteDiscountAsync(discountId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpPut]
        [Route("{discountId}")]
        public async Task<IActionResult> UpdateCategory(int discountId, DiscountUpdateDto discount)
        {
            var result = await _discountService.UpdateDiscountAsync(discountId, discount);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
