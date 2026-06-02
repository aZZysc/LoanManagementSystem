using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ProcessPaymentAsync(PaymentRequestDto dto)
        {
            if (dto.Amount <= 0)
                throw new ArgumentException("Payment amount must be greater than 0.");

            var loan = await _context.Loans
                .Include(l => l.Payments)
                .FirstOrDefaultAsync(l => l.Id == dto.LoanId);

            if (loan == null)
                throw new ArgumentException("Loan not found.");

            if (loan.Status == LoanStatus.Closed)
                throw new InvalidOperationException("Cannot process payments on a closed loan.");

            var payment = new Payment
            {
                LoanId = dto.LoanId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow
            };

            _context.Payments.Add(payment);

            // Check for loan closure
            decimal totalPaid = loan.Payments.Sum(p => p.Amount) + dto.Amount;
            decimal totalExpected = loan.MonthlyPayment * loan.TermMonths;

            if (totalPaid >= totalExpected)
            {
                loan.Status = LoanStatus.Closed;
            }

            await _context.SaveChangesAsync();
        }
    }
}