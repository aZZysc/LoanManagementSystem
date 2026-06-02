using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly AppDbContext _context; // For quick creation of a test user

        public CustomersController(ILoanService loanService, AppDbContext context)
        {
            _loanService = loanService;
            _context = context;
        }

        [HttpGet("loans")]
        public async Task<IActionResult> GetCustomerLoans([FromQuery] int customerId)
        {
            var loans = await _loanService.GetCustomerLoansAsync(customerId);
            return Ok(loans);
        }

        // Helper method to create a customer for testing purposes
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto dto)
        {
            var exists = await _context.Customers.AnyAsync(c => c.PersonalNumber == dto.PersonalNumber);
            if (exists) return BadRequest(new { error = "PersonalNumber must be unique." });

            var customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PersonalNumber = dto.PersonalNumber,
                BirthDate = dto.BirthDate,
                CreditScore = dto.CreditScore
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return StatusCode(201, customer);
        }
    }
}