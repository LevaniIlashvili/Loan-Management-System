using LoanManagementSystem.Domain.Entities;
using System.Data;

namespace LoanManagementSystem.Application.Interfaces.Infrastructure.Repositories;

public interface IRepaymentScheduleRepository
{
    Task CreateBatchAsync(IEnumerable<RepaymentSchedule> schedules);
}
