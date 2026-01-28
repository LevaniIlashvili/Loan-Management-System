namespace LoanManagementSystem.Application.Dtos;

public class SubmitLoanApplicationDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalNumber { get; set; } = string.Empty;
    public decimal MonthlyIncome { get; set; }
    public decimal Amount { get; set; }
    public int TermInMonths { get; set; }
}
