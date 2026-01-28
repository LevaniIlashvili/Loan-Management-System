using System.Data;

namespace LoanManagementSystem.Application.Interfaces.Infrastructure;

public interface IUnitOfWork
{
    IDbTransaction? Transaction { get; }

    void Begin();
    void Commit();
    void Rollback();
    Task ExecuteInTransactionAsync(Func<Task> action);
}
