using System;

namespace LoanManagementSystem.DTOs
{
    public record CustomerCreateDto(string FirstName, string LastName, string PersonalNumber, DateTime BirthDate, int CreditScore);

    public record LoanApplicationDto(int CustomerId, decimal Amount, int TermMonths);

    public record LoanResponseDto(
        int Id, int CustomerId, decimal Amount, decimal InterestRate,
        int TermMonths, decimal MonthlyPayment, string Status, DateTime CreatedAt);

    public record PaymentRequestDto(int LoanId, decimal Amount);
}