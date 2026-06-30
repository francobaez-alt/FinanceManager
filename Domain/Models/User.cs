using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set;  } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public string? ProfileImagen { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt {  get; set; }
        public DateTime? DeleteAt { get; set; }

        public Role Role { get; set; } = null!;
        public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<TransactionHistory> ModifiedHistories { get; set; } = new List<TransactionHistory>();



    }
}
