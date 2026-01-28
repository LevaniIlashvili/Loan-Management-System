using LoanManagementSystem.Domain.Enums;
using LoanManagementSystem.Domain.Exceptions;

namespace LoanManagementSystem.Domain.Entities;

public class LoanApplication
{
    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public decimal Amount { get; private set; }
    public int TermInMonths { get; private set; }
    public LoanApplicationStatus LoanStatus { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private LoanApplication() { }

    public static LoanApplication Create(Guid clientId, decimal amount, int term, decimal clientMonthlyIncome)
    {
        if (amount <= 0) throw new DomainException("Amount must be positive");
        if (term <= 0) throw new DomainException("Term must be positive");

        var monthlyPayment = amount / term;
        var maxAllowed = clientMonthlyIncome * 0.45m;

        var status = monthlyPayment > maxAllowed
            ? LoanApplicationStatus.Rejected
            : LoanApplicationStatus.Pending;

        return new LoanApplication
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            Amount = amount,
            TermInMonths = term,
            CreatedAt = DateTimeOffset.UtcNow,
            LoanStatus = status
        };
    }

    public List<RepaymentSchedule> Approve()
    {
        if (LoanStatus != LoanApplicationStatus.Pending)
            throw new DomainException("Only pending loans can be approved");

        LoanStatus = LoanApplicationStatus.Approved;

        var schedules = new List<RepaymentSchedule>();

        decimal monthlyBase = Math.Round(Amount / TermInMonths, 2);
        decimal totalAllocated = 0;

        DateTime nextDueDate = DateTime.UtcNow.AddMonths(1);

        for (int i = 0; i < TermInMonths; i++)
        {
            decimal currentPayment;

            if (i == TermInMonths - 1)
            {
                currentPayment = Amount - totalAllocated;
            }
            else
            {
                currentPayment = monthlyBase;
            }

            var schedule = RepaymentSchedule.Create(Id, nextDueDate, currentPayment);
            schedules.Add(schedule);

            totalAllocated += currentPayment;
            nextDueDate = nextDueDate.AddMonths(1);
        }

        return schedules;
    }

    public void Reject()
    {
        if (LoanStatus != LoanApplicationStatus.Pending)
            throw new DomainException("Only pending loans can be rejected");

        LoanStatus = LoanApplicationStatus.Rejected;
    }
}
