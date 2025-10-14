using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.IdentityEntities;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.UserService.Dtos;

namespace Store.Services.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IUserService> _logger;
        private readonly IMapper _mapper;
        public UserService(
            UserManager<ApplicationUser> userManager,
            ILogger<IUserService> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
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
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                    return response.Fail("400", string.Join(", ", result.Errors.Select(e => e.Description)));

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
                var users = await _userManager.Users.ToListAsync();
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
                var user = await _userManager.FindByIdAsync(userId);
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

                if (user.Address == null)
                    user.Address = new Address();

                if (!string.IsNullOrEmpty(dto.Street))
                    user.Address.Street = dto.Street;

                if (!string.IsNullOrEmpty(dto.City))
                    user.Address.City = dto.City;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return response.Fail("400", string.Join(", ", result.Errors.Select(e => e.Description)));

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
