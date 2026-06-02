using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost("CreateApplication")]
        public async Task<IActionResult> CreateApplication([FromBody] LoanApplicationDto dto)
        {
            var result = await _loanService.CreateApplicationAsync(dto);
            return StatusCode(201, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanStatus(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null) return NotFound(new { message = "Loan not found" });
            return Ok(loan);
        }
    }
}