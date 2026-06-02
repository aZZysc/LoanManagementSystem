using System.Collections.Generic;
using System.Threading.Tasks;
using LoanManagementSystem.DTOs;

namespace LoanManagementSystem.Services
{
    public interface ILoanService
    {
        Task<LoanResponseDto> CreateApplicationAsync(LoanApplicationDto dto);
        Task<LoanResponseDto?> GetLoanByIdAsync(int id);
        Task<IEnumerable<LoanResponseDto>> GetCustomerLoansAsync(int customerId);
    }
}