using System;
using System.Collections.Generic;

namespace LoanManagementSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PersonalNumber { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int CreditScore { get; set; }

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}