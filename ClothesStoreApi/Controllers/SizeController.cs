using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.ProductSizeService;
using Store.Services.Services.ProductSizeService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly IProductSizeService _sizeService;
        public SizeController(IProductSizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateSize(SizeCreateDto dto)
        {
            var result = await _sizeService.AddSizeAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllSizes()
        {
            var result = await _sizeService.GetAllSizeAsync();
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet]
        [Route("{sizeId}")]
        public async Task<IActionResult> GetSize(int sizeId)
        {
            var result = await _sizeService.GetSizeByIdAsync(sizeId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{sizeId}")]
        public async Task<IActionResult> DeleteSize(int sizeId)
        {
            var result = await _sizeService.DeleteSizeAsync(sizeId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpPut]
        [Route("{sizeId}")]
        public async Task<IActionResult> UpdateSize(int sizeId, SizeUpdateDto size)
        {
            var result = await _sizeService.UpdateSizeAsync(sizeId, size);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
