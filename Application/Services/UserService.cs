using Application.Common;
using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Application.Interfaces.Security;
using Application.Interfaces.Services;
using Application.Mappers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;


namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<UserDetailsDto>>> ListUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userDetailsDtos = _mapper.Map<IEnumerable<UserDetailsDto>>(users);
            if (userDetailsDtos.Count() == 0)
            {
                _logger.LogWarning("No users found.");
                return ApiResponse<IEnumerable<UserDetailsDto>>.Fail(
                    "No users found.",
                    HttpStatusCode.NotFound);
            }
            _logger.LogInformation("Users retrieved successfully. Count: {Count}", userDetailsDtos.Count());
            return ApiResponse<IEnumerable<UserDetailsDto>>.Ok(userDetailsDtos, "Users retrieved successfully", HttpStatusCode.OK);
        }
        public async Task<ApiResponse<UserDto>> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdWithRoleAsync(id);

            if (user is null)
            {
                _logger.LogWarning("User not found. Id: {Id}", id);
                return ApiResponse<UserDto>.Fail("User not found", HttpStatusCode.NotFound);
            }
            _logger.LogInformation("User retrieved successfully. Id: {Id}", id);
            return ApiResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), "User retrieved successfully", HttpStatusCode.OK);
        }
        public async Task<ApiResponse<bool>> ActivateUserAsync(int id)
        {
            var result = await _userRepository.ActiveAsync(id);
            if (!result)
            {
                _logger.LogWarning("User not found or could not be activated. Id: {Id}", id);
                return ApiResponse<bool>.Fail("User not found or could not be activated.", HttpStatusCode.NotFound);
            }
            _logger.LogInformation("User activated successfully. Id: {Id}", id);
            return ApiResponse<bool>.Ok(true, "User activated successfully", HttpStatusCode.NoContent);
        }
        public async Task<ApiResponse<bool>> DeactivateUserAsync(int id)
        {
            var result = await _userRepository.DeactivateAsync(id);

            if (!result)
            {
                _logger.LogWarning("User not found or could not be deactivated. Id: {Id}", id);
                return ApiResponse<bool>.Fail("User not found or could not be deactivated.", HttpStatusCode.NotFound);
            }
            _logger.LogInformation("User deactivated successfully. Id: {Id}", id);
            return ApiResponse<bool>.Ok(true, "User deactivated successfully", HttpStatusCode.NoContent);
        }

        public Task<ApiResponse<AdminUpdateUserDto>> AdminUpdateUserAsync(AdminUpdateUserDto adminUpdateUserDto)
        {
            throw new NotImplementedException();
        }


        public Task<ApiResponse<UserDto>> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }


        public Task<ApiResponse<UserDetailsDto>> GetUserDetailsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<ApiResponse<bool>> UpdatePasswordAsync(int id, UpdatePasswordDto updatePasswordDto)
        {
            var oldPasswordHash = await _userRepository.GetPasswordByIdAsync(id);

            if (oldPasswordHash is null)
            {
                _logger.LogWarning("User not found. Id: {Id}", id);
                return ApiResponse<bool>.Fail("User not found", HttpStatusCode.NotFound);
            }
            if (!_passwordHasher.Verify(updatePasswordDto.CurrentPassword, oldPasswordHash))
            {
                _logger.LogWarning("Old password is incorrect. Id: {Id}", id);
                return ApiResponse<bool>.Fail("Old password is incorrect", HttpStatusCode.BadRequest);
            }

            var result = await _userRepository.UpdatePasswordAsync(id, _passwordHasher.Hash(updatePasswordDto.NewPassword));
            if (!result)
            {
                _logger.LogError("Failed to update password, UserId: {Id}", id);
                return ApiResponse<bool>.Fail("Failed to update password", HttpStatusCode.InternalServerError);
            }

            _logger.LogInformation("Password updated successfully. Id: {Id}", id);
            return ApiResponse<bool>.Ok(true, "Password updated successfully", HttpStatusCode.NoContent);
        }

        public Task<ApiResponse<bool>> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            throw new NotImplementedException();
        }
    }
}
