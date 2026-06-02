using System.Threading.Tasks;
using LoanManagementSystem.DTOs;

namespace LoanManagementSystem.Services
{
    public interface IPaymentService
    {
        Task ProcessPaymentAsync(PaymentRequestDto dto);
    }
}