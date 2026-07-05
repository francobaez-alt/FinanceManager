using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Users
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfileImagen { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = null!;
    }
}
