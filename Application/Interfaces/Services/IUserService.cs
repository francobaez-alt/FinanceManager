using Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id);
    }
}
