namespace LoanManagementSystem.Web.Models;

public class RepaymentScheduleViewModel
{
    public Guid Id { get; set; }
    public Guid LoanApplicationId { get; set; }
    public DateTimeOffset DueDate { get; set; }
    public decimal AmountToPay { get; set; }
}
