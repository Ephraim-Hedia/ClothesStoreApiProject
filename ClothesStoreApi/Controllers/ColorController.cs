using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.ProductColorService;
using Store.Services.Services.ProductColorService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IProductColorService _colorService;
        public ColorController(IProductColorService colorService)
        {
            _colorService = colorService;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateColor(ColorCreateDto dto)
        {
            var result = await _colorService.AddColorAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllColors()
        {
            var result = await _colorService.GetAllColorAsync();
            return result.IsSuccess ? Ok(result) : NotFound(result); 
        }
        [HttpGet]
        [Route("{colorId}")]
        public async Task<IActionResult> GetColor(int colorId)
        {
            var result = await _colorService.GetColorByIdAsync(colorId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{colorId}")]
        public async Task<IActionResult> DeleteColor(int colorId)
        {
            var result = await _colorService.DeleteColorAsync(colorId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpPut]
        [Route("{colorId}")]
        public async Task<IActionResult> UpdateColor(int colorId, ColorUpdateDto color)
        {
            var result = await _colorService.UpdateColorAsync(colorId , color);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
