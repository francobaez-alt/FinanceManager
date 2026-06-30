using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class RolePermissions
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public Role Role { get; set; } = null!;

        public Permission Permission { get; set; } = null!;

    }
}
