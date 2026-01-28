using FluentValidation;
using LoanManagementSystem.Application.Dtos;

namespace LoanManagementSystem.Application.Validators;

public class SubmitLoanApplicationDtoValidator : AbstractValidator<SubmitLoanApplicationDto>
{
    public SubmitLoanApplicationDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters");

        RuleFor(x => x.PersonalNumber)
            .NotEmpty().WithMessage("Personal number is required")
            .Length(11).WithMessage("Personal number must be 11 digits");

        RuleFor(x => x.MonthlyIncome)
            .GreaterThan(0).WithMessage("Monthly income must be greater than 0");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Loan amount must be greater than 0")
            .LessThanOrEqualTo(100000).WithMessage("Loan amount cannot be more than 100,000");

        RuleFor(x => x.TermInMonths)
            .InclusiveBetween(1, 120).WithMessage("Term must be between 1 and 120 months");
    }
}
