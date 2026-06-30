using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();
    }
}
