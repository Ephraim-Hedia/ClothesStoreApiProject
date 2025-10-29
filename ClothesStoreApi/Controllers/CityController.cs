using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.CityService;
using Store.Services.Services.CityService.Dtos;

namespace Store.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddCity(CityCreateDto dto)
        {
            var result = await _cityService.AddCityAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCities()
        {
            var result = await _cityService.GetAllCityAsync();
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet]
        [Route("WithoutPricing")]
        public async Task<IActionResult> GetAllCitiesWithoutPricing()
        {
            var result = await _cityService.GetAllCityDropListAsync();
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet]
        [Route("{cityId}")]
        public async Task<IActionResult> GetCityById(int cityId)
        {
            var result = await _cityService.GetCityByIdAsync(cityId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{cityId}")]
        public async Task<IActionResult> DeleteCity(int cityId)
        {
            var result = await _cityService.DeleteCityAsync(cityId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpPut]
        [Route("{cityId}")]
        public async Task<IActionResult> UpdateColor(int cityId, CityUpdateDto city)
        {
            var result = await _cityService.UpdateCityAsync(cityId, city);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
