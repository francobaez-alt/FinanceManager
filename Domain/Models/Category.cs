using Domain.Enums;

namespace Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public CategoryType Type { get; set; }
        public string? Icon {  get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt {  get; set; }

        public User User { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<ScheduledTransaction> ScheduledTransactions { get; set; } = new List<ScheduledTransaction>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    }
}
