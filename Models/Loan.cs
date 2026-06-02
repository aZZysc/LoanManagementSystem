using System;
using System.Collections.Generic;

namespace LoanManagementSystem.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; } // For example, 0.12 for 12%
        public int TermMonths { get; set; }
        public decimal MonthlyPayment { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<LoanSchedule> Schedules { get; set; } = new List<LoanSchedule>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}