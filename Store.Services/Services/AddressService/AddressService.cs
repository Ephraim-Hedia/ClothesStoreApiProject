using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.IdentityEntities;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.AddressService.Dtos;
using System.Net;
using System.Runtime.InteropServices;

namespace Store.Services.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IAddressService> _logger;
        private readonly IMapper _mapper;
        public AddressService(
            UserManager<ApplicationUser> userManager,
            ILogger<IAddressService> logger,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
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
                var user = await _userManager.Users
                    .Include(u => u.Address)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    return response.Fail("404", $"User not found with Id: {userId}");

                var address = _mapper.Map<Address>(dto);
                address.ApplicationUserId = user.Id;
                user.Address ??= new List<Address>();
                if(user.Address.Count >=3)
                    return response.Fail("404", $"User with Id: {userId}, Have Max Limit of Addresses, try to remove one first, then add");

                user.Address.Add(address);

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
                var user = await _userManager.Users
                                    .Include(u => u.Address)
                                    .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    return response.Fail("400", "Invalid Data, Not Valid User Id");

                var address = user.Address?.FirstOrDefault(a => a.Id == addressId);
                if (address == null)
                    return response.Fail("404", $"Address not found with Id: {addressId}");

                user.Address.Remove(address);
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
                var user = await _userManager.Users
                    .Include(user => user.Address)
                    .FirstOrDefaultAsync(user => user.Id == userId);
                if (user == null)
                    return response.Fail("404", $"User not found with Id: {userId}");
                var address = user.Address.FirstOrDefault(a => a.Id == addressId);
                if (address == null)
                    return response.Fail("404", $"Address not found with Id: {addressId}");

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
                var user = await _userManager.Users
                    .Include(user => user.Address)
                    .FirstOrDefaultAsync(user => user.Id == userId);
                if(user == null)
                    return response.Fail("404", $"User not found with Id: {userId}");

                var addresses = user.Address ?? new List<Address>();
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
            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return response.Fail("404", $"User not found with Id: {userId}");

            var address = user.Address?.FirstOrDefault(a => a.Id == addressId);
            if (address == null)
                return response.Fail("404", $"Address not found with Id: {addressId}");

            if (!string.IsNullOrEmpty(dto.Street)) address.Street = dto.Street;
            if (!string.IsNullOrEmpty(dto.City)) address.City = dto.City;
            if (!string.IsNullOrEmpty(dto.State)) address.State = dto.State;
            if (!string.IsNullOrEmpty(dto.ZipCode)) address.ZipCode = dto.ZipCode;

            await _userManager.UpdateAsync(user);

            return response.Success(_mapper.Map<AddressResultDto>(address));
        }
    }
}
