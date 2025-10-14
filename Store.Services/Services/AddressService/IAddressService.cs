using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.AddressService.Dtos;

namespace Store.Services.Services.AddressService
{
    public interface IAddressService
    {
        Task<CommonResponse<AddressResultDto>> AddAddressToUserAsync(string userId, AddressCreateDto dto);
        Task<CommonResponse<AddressResultDto>> UpdateAddressAsync(string userId, int addressId, AddressUpdateDto dto);
        Task<CommonResponse<IReadOnlyList<AddressResultDto>>> GetAddressesByUserIdAsync(string userId);
        Task<CommonResponse<AddressResultDto>> GetAddressByIdAsync(string userId, int addressId);
        Task<CommonResponse<bool>> DeleteAddressAsync(string userId, int addressId);
    }
}
