using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.AddressService;
using Store.Services.Services.AddressService.Dtos;
using System.Security.Claims;

namespace Store.Api.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> AddAddressToUserAsync(AddressCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Invalid or missing token." });

            var result = await _addressService.AddAddressToUserAsync(userId, dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        [Route("{addressId}")]
        public async Task<IActionResult> UpdateAddressAsync(int addressId, AddressUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Invalid or missing token." });
            var result = await _addressService.UpdateAddressAsync(userId, addressId, dto);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAddressesByUserIdAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Invalid or missing token." });

            var result = await _addressService.GetAddressesByUserIdAsync(userId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpGet]
        [Route("{addressId}")]
        public async Task<IActionResult> GetAddressByIdAsync(int addressId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Invalid or missing token." });


            var result = await _addressService.GetAddressByIdAsync(userId , addressId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{addressId}")]
        public async Task<IActionResult> DeleteAddressAsync(int addressId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Invalid or missing token." });

            var result = await _addressService.DeleteAddressAsync(userId, addressId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
