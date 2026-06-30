using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public int WalletId { get; set; }
        public int CategoryId { get; set; }
        public TransactionType Type {  get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public Wallet Wallet { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
    }
}
