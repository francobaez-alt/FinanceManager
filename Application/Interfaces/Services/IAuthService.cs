using Application.Common;
using Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterUserDto dto);

        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginUserDto dto);
    }
}
