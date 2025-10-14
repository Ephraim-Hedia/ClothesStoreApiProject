using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.AddressService;
using Store.Services.Services.AddressService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddAddressToUserAsync([FromQuery] string userId, AddressCreateDto dto)
        {
            var result = await _addressService.AddAddressToUserAsync(userId, dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        [Route("{addressId}")]
        public async Task<IActionResult> UpdateAddressAsync([FromQuery] string userId, int addressId, AddressUpdateDto dto)
        {
            var result = await _addressService.UpdateAddressAsync(userId, addressId, dto);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAddressesByUserIdAsync([FromQuery] string userId)
        {
            var result = await _addressService.GetAddressesByUserIdAsync(userId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpGet]
        [Route("{addressId}")]
        public async Task<IActionResult> GetAddressByIdAsync([FromQuery]string userId, int addressId)
        {
            var result = await _addressService.GetAddressByIdAsync(userId , addressId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{addressId}")]
        public async Task<IActionResult> DeleteAddressAsync([FromQuery] string userId, int addressId)
        {
            var result = await _addressService.DeleteAddressAsync(userId, addressId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
