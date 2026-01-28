using LoanManagementSystem.Domain.Exceptions;

namespace LoanManagementSystem.Domain.Entities;

public class RepaymentSchedule
{
    public Guid Id { get; private set; }
    public Guid LoanApplicationId { get; private set; }
    public DateTimeOffset DueDate { get; private set; }
    public decimal AmountToPay { get; private set; }

    private RepaymentSchedule() { }

    internal static RepaymentSchedule Create(Guid loanId, DateTime dueDate, decimal amount)
    {
        if (amount <= 0) throw new DomainException("Repayment amount must be positive.");

        return new RepaymentSchedule
        {
            Id = Guid.NewGuid(),
            LoanApplicationId = loanId,
            DueDate = dueDate,
            AmountToPay = amount
        };
    }
}
