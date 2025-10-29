using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.IdentityEntities;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.AddressService.Dtos;
using Store.Services.Services.CityService;

namespace Store.Services.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IAddressService> _logger;
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        public AddressService(
            UserManager<ApplicationUser> userManager,
            ICityService cityService,
            ILogger<IAddressService> logger,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _cityService = cityService;
        }
        public async Task<CommonResponse<AddressResultDto>> AddAddressToUserAsync(string userId, AddressCreateDto dto)
        {
            var response = new CommonResponse<AddressResultDto>();
            if (userId == null)
                return response.Fail("400", "Invalid Data, userId is Null");
            if (dto == null)
                return response.Fail("400", "Invalid Data, Address Data is Null");

            try
            {
                // Check the user existance 
                var user = await _userManager.Users
                    .Include(u => u.Addresses)
                    .ThenInclude(a => a.City)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    return response.Fail("404", $"User not found with Id: {userId}");

                var result = await _cityService.GetCityByIdAsync(dto.CityId);
                if(!result.IsSuccess)
                    return response.Fail("404", $"City not found with Id: {dto.CityId}");

                // Convert Address Dto to Address to can add it to the database 
                var address = _mapper.Map<Address>(dto);
                address.ApplicationUserId = user.Id;

                // check if the user doesn't have any address, create empty list of addresses for him
                user.Addresses ??= new List<Address>();
                // check if the user have 3 address or more, don't add the new address to the database until the user delete old address
                if(user.Addresses.Count >=3)
                    return response.Fail("404", $"User with Id: {userId}, Have Max Limit of Addresses, try to remove one first, then add");

                // if the checks true, add the address to the database 
                user.Addresses.Add(address);
                await _userManager.UpdateAsync(user);

                return response.Success(_mapper.Map<AddressResultDto>(address));
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
            
        }

        public async Task<CommonResponse<bool>> DeleteAddressAsync(string userId, int addressId)
        {
            var response = new CommonResponse<bool>();
            if (addressId <= 0)
                return response.Fail("400", "Invalid Data, address Id should be more than 0");
            try
            {
                // check existance of the user 
                var user = await _userManager.Users
                                    .Include(u => u.Addresses)
                                    .ThenInclude(a => a.City)
                                    .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    return response.Fail("400", "Invalid Data, Not Valid User Id");

                // check existance of the address
                var address = user.Addresses?.FirstOrDefault(a => a.Id == addressId);
                if (address == null)
                    return response.Fail("404", $"Address not found with Id: {addressId}");

                // if everything is good, remove the address from the user 
                user.Addresses.Remove(address);
                await _userManager.UpdateAsync(user);
                return response.Success(true);

            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }

        }

        public async Task<CommonResponse<AddressResultDto>> GetAddressByIdAsync(string userId, int addressId)
        {
            var response = new CommonResponse<AddressResultDto>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, user Id is null");

            if(addressId <= 0)
                return response.Fail("400", "Invalid Data, address Id should be more than 0");

            try
            {
                // check existance of the user 
                var user = await _userManager.Users
                    .Include(user => user.Addresses)
                    .ThenInclude(a => a.City)
                    .FirstOrDefaultAsync(user => user.Id == userId);
                if (user == null)
                    return response.Fail("404", $"User not found with Id: {userId}");

                // check existance of the address 
                var address = user.Addresses.FirstOrDefault(a => a.Id == addressId);
                if (address == null)
                    return response.Fail("404", $"Address not found with Id: {addressId}");

                // if everything is good return the address after mapping
                return response.Success(_mapper.Map<AddressResultDto>(address));
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<IReadOnlyList<AddressResultDto>>> GetAddressesByUserIdAsync(string userId)
        {
            var response = new CommonResponse<IReadOnlyList<AddressResultDto>>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400","Invalid Data, User Id is Null");

            try
            {
                // check the existance of the user 
                var user = await _userManager.Users
                    .Include(user => user.Addresses)
                    .ThenInclude(a => a.City)
                    .FirstOrDefaultAsync(user => user.Id == userId);
                if(user == null)
                    return response.Fail("404", $"User not found with Id: {userId}");

                // check if the user doesn't have any address, create empty list of the addresses
                var addresses = user.Addresses ?? new List<Address>();
                // return the addresses of the user after mapping 
                return response.Success(_mapper.Map<IReadOnlyList<AddressResultDto>>(addresses));
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<AddressResultDto>> UpdateAddressAsync(string userId, int addressId, AddressUpdateDto dto)
        {
            var response = new CommonResponse<AddressResultDto>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, User Id is null");
            if (addressId <= 0)
                return response.Fail("400", "Invalid Data, Address Id must be more than 0");

            try
            {
                // check existance of the user
                var user = await _userManager.Users
                .Include(u => u.Addresses)
                .ThenInclude(a => a.City)
                .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    return response.Fail("404", $"User not found with Id: {userId}");
                

                // check existance of the address
                var address = user.Addresses?.FirstOrDefault(a => a.Id == addressId);
                if (address == null)
                    return response.Fail("404", $"Address not found with Id: {addressId}");


                // update address
                if (dto.CityId != null)
                {
                    var result = await _cityService.GetCityByIdAsync(dto.CityId.Value);
                    if (!result.IsSuccess)
                        return response.Fail("404", $"City not found with Id: {dto.CityId}");
                    address.CityId = dto.CityId.Value;
                }            
                if (!string.IsNullOrEmpty(dto.District)) address.District = dto.District;
                if (!string.IsNullOrEmpty(dto.Street)) address.Street = dto.Street;
                if (!string.IsNullOrEmpty(dto.ApartmentNumber)) address.ApartmentNumber = dto.ApartmentNumber;
                if (!string.IsNullOrEmpty(dto.FloorNumber)) address.FloorNumber = dto.FloorNumber;
                if (!string.IsNullOrEmpty(dto.Landmark)) address.Landmark = dto.Landmark;
                if (!string.IsNullOrEmpty(dto.RecipientName)) address.RecipientName = dto.RecipientName;
                if (!string.IsNullOrEmpty(dto.PhoneNumber)) address.PhoneNumber = dto.PhoneNumber;
                
                await _userManager.UpdateAsync(user);

                // return response with mapped address
                return response.Success(_mapper.Map<AddressResultDto>(address));
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
            
        }
    }
}
