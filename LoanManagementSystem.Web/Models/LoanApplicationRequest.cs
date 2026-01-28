using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Web.Models;

public class LoanApplicationRequest
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "Name shouldn't be longer than 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Personal number is required")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Personal number should be 11 digits")]
    public string PersonalNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Monthly income is required")]
    [Range(0.01, 1000000, ErrorMessage = "Monthly income should be greater than 0")]
    public decimal MonthlyIncome { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    [Range(100, 50000, ErrorMessage = "Amount should be between 100 and 50000")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Term in months is requred")]
    [Range(1, 120, ErrorMessage = "Term in months should be between 1 and 120")]
    public int TermInMonths { get; set; }
}