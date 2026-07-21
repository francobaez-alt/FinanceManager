using Application.Common;
using Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ApiResponse<UserDto>> GetByIdAsync(int id);
        Task<ApiResponse<IEnumerable<UserDetailsDto>>> ListUsersAsync();
        Task<ApiResponse<UserDetailsDto>> GetUserDetailsByIdAsync(int id);
        Task<ApiResponse<bool>> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<ApiResponse<AdminUpdateUserDto>> AdminUpdateUserAsync(AdminUpdateUserDto adminUpdateUserDto);
        Task<ApiResponse<bool>> DesactiveUserAsync(int id);
        Task<ApiResponse<bool>> ActivateUserAsync(int id);
        Task<ApiResponse<UserDto>> GetByEmailAsync(string email);
        Task<ApiResponse<bool>> UpdatePasswordAsync(int id, UpdatePasswordDto updatePasswordDto);
        
        

    }
}
