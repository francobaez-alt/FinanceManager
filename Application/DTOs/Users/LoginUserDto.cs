using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Users
{
    public class LoginUserDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
