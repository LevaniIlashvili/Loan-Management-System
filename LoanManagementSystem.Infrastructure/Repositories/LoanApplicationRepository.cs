using Dapper;
using LoanManagementSystem.Application.Dtos;
using LoanManagementSystem.Application.Interfaces.Infrastructure;
using LoanManagementSystem.Application.Interfaces.Infrastructure.Repositories;
using LoanManagementSystem.Domain.Entities;
using System.Data;

namespace LoanManagementSystem.Infrastructure.Repositories;

public class LoanApplicationRepository : ILoanApplicationRepository
{
    private readonly IDbConnection _connection;
    private readonly IUnitOfWork _uow;

    public LoanApplicationRepository(IDbConnection connection, IUnitOfWork uow)
    {
        _connection = connection;
        _uow = uow;
    }

    public async Task<LoanApplication?> GetByIdAsync(Guid id)
    {
        const string sql = @"SELECT * FROM LoanApplications WHERE Id = @Id";

        return await _connection.QueryFirstOrDefaultAsync<LoanApplication>(sql, new { Id = id }, _uow.Transaction);
    }

    public async Task<LoanApplicationDetailsDto?> GetLoanApplicationDetailsAsync(Guid id)
    {
        const string sql = @"
            SELECT 
                l.Id, l.Amount, l.TermInMonths, l.LoanStatus, l.CreatedAt,
                r.Id, r.LoanApplicationId, r.DueDate, r.AmountToPay
            FROM LoanApplications l
            LEFT JOIN RepaymentSchedules r ON l.Id = r.LoanApplicationId
            WHERE l.Id = @Id
            ORDER BY r.DueDate ASC;
        ";

        LoanApplicationDetailsDto? loanEntry = null;

        await _connection.QueryAsync<LoanApplicationDetailsDto, RepaymentScheduleDto, LoanApplicationDetailsDto>(
            sql,
            (loan, schedule) =>
            {
                if (loanEntry == null)
                {
                    loanEntry = loan;
                }

                if (schedule != null)
                {
                    loanEntry.RepaymentSchedules.Add(schedule);
                }

                return loanEntry;
            },
            new { Id = id },
            transaction: _uow.Transaction,
            splitOn: "Id"
        );

        return loanEntry;
    }

    public async Task<Guid> CreateAsync(LoanApplication loanApplication)
    {
        const string sql = @"
            INSERT INTO LoanApplications (Id, ClientId, Amount, TermInMonths, LoanStatus)
            VALUES (@Id, @ClientId, @Amount, @TermInMonths, @LoanStatus);
        ";

        await _connection.ExecuteAsync(sql, loanApplication, _uow.Transaction);

        return loanApplication.Id;
    }

    public async Task<IEnumerable<LoanApplicationDto>> GetAllWithClientsAsync()
    {
        const string sql = @"
            SELECT 
                la.Id,
                la.ClientId,
                la.Amount,
                la.TermInMonths,
                la.LoanStatus,
                la.CreatedAt,
                c.FirstName AS ClientFirstName,
                c.LastName AS ClientLastName,
                c.PersonalNumber AS ClientPersonalNumber,
                c.MonthlyIncome AS ClientMonthlyIncome 
            FROM LoanApplications la
            INNER JOIN Clients c ON la.ClientId = c.Id
            ORDER BY la.CreatedAt DESC
        ";

        return await _connection.QueryAsync<LoanApplicationDto>(sql, _uow.Transaction);
    }

    public async Task UpdateAsync(LoanApplication loanApplication)
    {
        const string sql = @"
            UPDATE LoanApplications
            SET 
                Amount = @Amount,
                TermInMonths = @TermInMonths,
                LoanStatus = @LoanStatus
            WHERE Id = @Id;
        ";

        await _connection.ExecuteAsync(sql, loanApplication, _uow.Transaction);
    }
}
