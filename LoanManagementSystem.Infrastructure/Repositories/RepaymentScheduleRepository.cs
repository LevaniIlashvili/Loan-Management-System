using Dapper;
using LoanManagementSystem.Application.Interfaces.Infrastructure;
using LoanManagementSystem.Application.Interfaces.Infrastructure.Repositories;
using LoanManagementSystem.Domain.Entities;
using System.Data;

namespace LoanManagementSystem.Infrastructure.Repositories;

public class RepaymentScheduleRepository : IRepaymentScheduleRepository
{
    private readonly IDbConnection _connection;
    private readonly IUnitOfWork _uow;

    public RepaymentScheduleRepository(IDbConnection connection, IUnitOfWork uow)
    {
        _connection = connection;
        _uow = uow;
    }

    public async Task CreateBatchAsync(IEnumerable<RepaymentSchedule> schedules)
    {
        const string sql = @"
            INSERT INTO RepaymentSchedules (Id, LoanApplicationId, DueDate, AmountToPay)
            VALUES (@Id, @LoanApplicationId, @DueDate, @AmountToPay);
        ";

        await _connection.ExecuteAsync(sql, schedules, _uow.Transaction);
    }
}
