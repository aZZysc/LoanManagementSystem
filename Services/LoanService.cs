using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Services
{
    public class LoanService : ILoanService
    {
        private readonly AppDbContext _context;

        public LoanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoanResponseDto> CreateApplicationAsync(LoanApplicationDto dto)
        {
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                throw new ArgumentException("Customer not found.");

            // Age validation
            var age = DateTime.UtcNow.Year - customer.BirthDate.Year;
            if (customer.BirthDate.Date > DateTime.UtcNow.AddYears(-age)) age--;
            if (age < 18)
                throw new InvalidOperationException("Customer must be at least 18 years old.");

            // Loan parameters validation
            if (dto.Amount < 500 || dto.Amount > 50000)
                throw new ArgumentException("Amount must be between 500 and 50,000.");
            if (dto.TermMonths < 6 || dto.TermMonths > 60)
                throw new ArgumentException("Term must be between 6 and 60 months.");

            // Credit scoring
            var status = customer.CreditScore < 300 ? LoanStatus.Rejected : LoanStatus.Approved;
            decimal interestRate = 0.15m; // Base rate 15%

            var loan = new Loan
            {
                CustomerId = dto.CustomerId,
                Amount = dto.Amount,
                InterestRate = interestRate,
                TermMonths = dto.TermMonths,
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            if (status == LoanStatus.Approved)
            {
                // PMT calculation (Annuity payment)
                decimal monthlyRate = interestRate / 12;
                double pmt = (double)dto.Amount * ((double)monthlyRate * Math.Pow(1 + (double)monthlyRate, dto.TermMonths)) /
                             (Math.Pow(1 + (double)monthlyRate, dto.TermMonths) - 1);

                loan.MonthlyPayment = Math.Round((decimal)pmt, 2);

                // LoanSchedule generation
                for (int i = 1; i <= dto.TermMonths; i++)
                {
                    loan.Schedules.Add(new LoanSchedule
                    {
                        PMT = loan.MonthlyPayment,
                        Date = DateTime.UtcNow.AddMonths(i)
                    });
                }
            }

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return MapToDto(loan);
        }

        public async Task<LoanResponseDto?> GetLoanByIdAsync(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            return loan == null ? null : MapToDto(loan);
        }

        public async Task<IEnumerable<LoanResponseDto>> GetCustomerLoansAsync(int customerId)
        {
            return await _context.Loans
                .Where(l => l.CustomerId == customerId)
                .Select(l => MapToDto(l))
                .ToListAsync();
        }

        private static LoanResponseDto MapToDto(Loan l) =>
            new(l.Id, l.CustomerId, l.Amount, l.InterestRate, l.TermMonths, l.MonthlyPayment, l.Status.ToString(), l.CreatedAt);
    }
}