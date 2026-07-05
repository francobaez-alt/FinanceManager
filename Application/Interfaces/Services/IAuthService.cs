using Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginUserDto loginUserDto);
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerUserDto);
    }
}
