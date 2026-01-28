using LoanManagementSystem.Domain.Enums;

namespace LoanManagementSystem.Application.Dtos;

public class LoanApplicationDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public decimal Amount { get; set; }
    public short TermInMonths { get; set; }
    public LoanApplicationStatus LoanStatus { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string ClientFirstName { get; set; }
    public string ClientLastName { get; set; }
    public string ClientPersonalNumber { get; set; }
    public decimal ClientMonthlyIncome { get; set; }
}