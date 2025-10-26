using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.IdentityEntities;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.AddressService;
using Store.Services.Services.AddressService.Dtos;
using Store.Services.Services.UserService.Dtos;

namespace Store.Services.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAddressService _addressService;
        private readonly ILogger<IUserService> _logger;
        private readonly IMapper _mapper;
        public UserService(
            UserManager<ApplicationUser> userManager,
            ILogger<IUserService> logger,
            IMapper mapper,
            IAddressService addressService)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _addressService = addressService;
        }
        public async Task<CommonResponse<UserResultDto>> CreateUserAsync(UserCreateDto dto)
        {
            var response = new CommonResponse<UserResultDto>();
            if (dto == null)
                return response.Fail("400", "Invalid Data, User Data is null");
            if ( string.IsNullOrEmpty(dto.UserName))
                return response.Fail("400", "Invalid Data, User Name is Required");
            if (string.IsNullOrEmpty(dto.Email))
                return response.Fail("400", "Invalid Data, User Email is Required");
            if (string.IsNullOrEmpty(dto.Password))
                return response.Fail("400", "Invalid Data, Password is Required");
            if (string.IsNullOrEmpty(dto.PhoneNumber))
                return response.Fail("400", "Invalid Data, Phone Number is Required");
            if (string.IsNullOrEmpty(dto.City))
                return response.Fail("400", "Invalid Data, City is Required");

            try
            {
                var user = _mapper.Map<ApplicationUser>(dto);            
                var result = await _userManager.CreateAsync(user , dto.Password);
                if (!result.Succeeded)
                    return response.Fail("400", string.Join(", ", result.Errors.Select(e => e.Description)));

                // Optional: Add Address if data provided
                if (!string.IsNullOrEmpty(dto.Street) || !string.IsNullOrEmpty(dto.City))
                {
                    var addressDto = new AddressCreateDto
                    {
                        Street = dto.Street,
                        City = dto.City,
                        State = dto.State,
                        ZipCode = dto.ZipCode
                    };

                    await _addressService.AddAddressToUserAsync(user.Id, addressDto);
                }

                var mappedUser = _mapper.Map<UserResultDto>(user);
                return response.Success(mappedUser);

            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }

        }

        public async Task<CommonResponse<bool>> DeleteUserAsync(string userId)
        {
            var response = new CommonResponse<bool>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, User Id is null");
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return response.Fail("404", $"Not Found User With Id : {userId}");
                
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return response.Fail("400", string.Join(", ", result.Errors.Select(e => e.Description)));

                return response.Success(true);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<IReadOnlyList<UserResultDto>>> GetAllUsersAsync()
        {
            var response = new CommonResponse<IReadOnlyList<UserResultDto>>();
            try
            {
                var users = await _userManager.Users.Include(user => user.Addresses).ToListAsync();
                if (!users.Any())
                    return response.Fail("404", "Not Found Users");
                var mappedUsers = _mapper.Map<IReadOnlyList<UserResultDto>>(users);
                return response.Success(mappedUsers);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }

        }

        public async Task<CommonResponse<UserResultDto>> GetUserByIdAsync(string userId)
        {
            var response = new CommonResponse<UserResultDto>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, User Id is null");
            try
            {
                var user = await _userManager.Users
                    .Include(user => user.Addresses)
                    .FirstOrDefaultAsync(user => user.Id == userId);
                if (user == null)
                    return response.Fail("404", $"Not Found User With Id : {userId}");
                var mappedUser = _mapper.Map<UserResultDto>(user);
                return response.Success(mappedUser);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<UserResultDto>> UpdateUserAsync(string userId, UserUpdateDto dto)
        {
            var response = new CommonResponse<UserResultDto>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, user Id is null");
            if(dto == null)
                return response.Fail("400", "Invalid Data, user data is null");

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return response.Fail("404", $"User not found with Id : {userId}");
                if (!string.IsNullOrEmpty(dto.UserName))
                    user.UserName = dto.UserName;

                if (!string.IsNullOrEmpty(dto.PhoneNumber))
                    user.PhoneNumber = dto.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return response.Fail("400", string.Join(", ", result.Errors.Select(e => e.Description)));

                // Address update handled via AddressService (only if you want)
                if (!string.IsNullOrEmpty(dto.Street) || !string.IsNullOrEmpty(dto.City))
                {
                    var addresses = await _addressService.GetAddressesByUserIdAsync(userId);
                    if (addresses.IsSuccess && addresses.Data.Any())
                    {
                        var firstAddress = addresses.Data.First();
                        var updateDto = new AddressUpdateDto
                        {
                            Street = dto.Street,
                            City = dto.City,
                            State = dto.State,
                            ZipCode = dto.ZipCode
                        };

                        await _addressService.UpdateAddressAsync(userId, firstAddress.Id, updateDto);
                    }
                    else
                    {
                        // Add new address if none exists
                        var createDto = new AddressCreateDto
                        {
                            Street = dto.Street,
                            City = dto.City,
                            State = dto.State,
                            ZipCode = dto.ZipCode
                        };

                        await _addressService.AddAddressToUserAsync(userId, createDto);
                    }
                }


                var mappedUser = _mapper.Map<UserResultDto>(user);
                return response.Success(mappedUser);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }


        }
    }
}
