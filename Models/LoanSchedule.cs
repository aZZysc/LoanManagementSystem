using System;

namespace LoanManagementSystem.Models
{
    public class LoanSchedule
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public Loan? Loan { get; set; }
        public decimal PMT { get; set; } // Monthly payment amount
        public DateTime Date { get; set; }
    }
}