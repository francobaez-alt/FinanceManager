using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Users
{
    public class AdminUpdateUserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

    }
}
