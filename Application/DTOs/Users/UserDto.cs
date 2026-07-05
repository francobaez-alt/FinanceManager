using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfileImagen { get; set; }
        public DateTime? LastLogin  { get; set; }
        public string Role { get; set; } = null!;
    }
}
