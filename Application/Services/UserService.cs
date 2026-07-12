using Application.Common;
using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using AutoMapper;
using System.Net;


namespace Application.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public UserService(
            IUserRepository userRepository, 
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<UserDetailsDto>>> ListUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userDetailsDtos = _mapper.Map<IEnumerable<UserDetailsDto>>(users);
            return ApiResponse<IEnumerable<UserDetailsDto>>.Ok(userDetailsDtos, "Users retrieved successfully", HttpStatusCode.OK);
        }
           
        
        public async Task<ApiResponse<UserDto>> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdWithRoleAsync(id);

            if (user is null)
                return ApiResponse<UserDto>.Fail("User not found", HttpStatusCode.NotFound);

            return ApiResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), "User retrieved successfully", HttpStatusCode.OK);
        }
        public async Task<ApiResponse<bool>> ActivateUserAsync(int id)
        {
            var result = await _userRepository.ActiveAsync(id);
            if (!result)
            {
                return ApiResponse<bool>.Fail("User not found or could not be activated.", HttpStatusCode.NotFound);
            }

            return ApiResponse<bool>.Ok(true, "User activated successfully", HttpStatusCode.OK);
        }
        public async Task<ApiResponse<bool>> DesactiveUserAsync(int id)
        {
            var result = await _userRepository.DesactiveAsync(id);

            if(!result)
                return ApiResponse<bool>.Fail("User not found or could not be deactivated.", HttpStatusCode.NotFound);

            return ApiResponse<bool>.Ok(true, "User deactivated successfully", HttpStatusCode.OK);
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


        public Task<ApiResponse<UserDto>> UpdatePasswordAsync(UpdatePasswordDto updatePasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<UpdateUserDto>> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            throw new NotImplementedException();
        }
    }
}
