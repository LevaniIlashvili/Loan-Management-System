using LoanManagementSystem.Application.Interfaces.Infrastructure;
using System.Data;

namespace LoanManagementSystem.Infrastructure;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IDbConnection _connection;
    private IDbTransaction? _transaction;

    public UnitOfWork(IDbConnection connection)
    {
        _connection = connection;
    }

    public IDbTransaction? Transaction => _transaction;

    public void Begin()
    {
        if (_connection.State != ConnectionState.Open)
            _connection.Open();

        _transaction = _connection.BeginTransaction();
    }

    public void Commit()
    {
        try { _transaction?.Commit(); }
        finally { Dispose(); }
    }

    public void Rollback()
    {
        try { _transaction?.Rollback(); }
        finally { Dispose(); }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _transaction = null;
    }

    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
    {
        try
        {
            Begin();
            var result = await action();
            Commit();
            return result;
        }
        catch
        {
            Rollback();
            throw;
        }
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        try
        {
            Begin();
            await action();
            Commit();
        }
        catch
        {
            Rollback();
            throw;
        }
    }
}
