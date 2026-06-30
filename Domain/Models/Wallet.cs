using Domain.Enums;

namespace Domain.Models
{
    public class Wallet
    { 
        public int Id { get; set; }
        public int UserId { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<ScheduledTransaction> ScheduledTransactions { get; set; } = new List<ScheduledTransaction>();
    }
}
