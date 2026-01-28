using LoanManagementSystem.Domain.Enums;

namespace LoanManagementSystem.Application.Dtos;

public class LoanApplicationDetailsDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public decimal Amount { get; set; }
    public int TermInMonths { get; set; }
    public LoanApplicationStatus LoanStatus { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public List<RepaymentScheduleDto> RepaymentSchedules { get; set; } = new List<RepaymentScheduleDto>();
}
