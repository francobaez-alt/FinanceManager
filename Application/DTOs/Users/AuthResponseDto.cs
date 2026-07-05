using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Users
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
