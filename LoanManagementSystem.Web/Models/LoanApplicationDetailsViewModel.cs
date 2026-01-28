namespace LoanManagementSystem.Web.Models;

public class LoanApplicationDetailsViewModel
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public decimal Amount { get; set; }
    public int TermInMonths { get; set; }
    public LoanApplicationStatus LoanStatus { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public List<RepaymentScheduleViewModel> RepaymentSchedules { get; set; }
}