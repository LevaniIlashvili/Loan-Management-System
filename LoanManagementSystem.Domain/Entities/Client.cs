using LoanManagementSystem.Domain.Exceptions;

namespace LoanManagementSystem.Domain.Entities;

public class Client
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PersonalNumber { get; private set; }
    public decimal MonthlyIncome { get; private set; }

    private Client() { }

    public static Client Create(string firstName, string lastName, string personalNumber, decimal monthlyIncome)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new DomainException("First name is required");
        if (string.IsNullOrWhiteSpace(lastName)) throw new DomainException("Last name is required");

        if (string.IsNullOrWhiteSpace(personalNumber) || personalNumber.Length != 11)
            throw new DomainException("Personal number must be exactly 11 digits");

        if (monthlyIncome < 0) throw new DomainException("Monthly income cannot be negative");

        return new Client
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            PersonalNumber = personalNumber,
            MonthlyIncome = monthlyIncome
        };
    }

    public void UpdateIncome(decimal newIncome)
    {
        if (newIncome < 0) throw new DomainException("New income cannot be negative");
        MonthlyIncome = newIncome;
    }
}
