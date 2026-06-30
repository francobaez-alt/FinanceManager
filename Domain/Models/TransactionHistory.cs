using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class TransactionHistory
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public int? ModifiedByUserId { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal NewAmount { get; set; } 
        public string? PreviousDescription { get; set; }
        public string? NewDescription { get; set; }
        public DateTime ModifiedAt { get; set; }

        public Transaction Transaction { get; set; } = null!;
        public User? ModifiedByUser { get; set; }
        
    }
}
