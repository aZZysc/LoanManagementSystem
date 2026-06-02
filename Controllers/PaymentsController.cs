using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Services;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] PaymentRequestDto dto)
        {
            await _paymentService.ProcessPaymentAsync(dto);
            return Ok(new { message = "Payment successfully processed." });
        }
    }
}