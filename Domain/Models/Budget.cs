using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; } = null!;
        public Category Category { get; set; } = null!;
    } 
}
