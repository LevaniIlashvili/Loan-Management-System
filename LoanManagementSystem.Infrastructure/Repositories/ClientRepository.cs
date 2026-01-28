using Dapper;
using LoanManagementSystem.Application.Interfaces.Infrastructure;
using LoanManagementSystem.Application.Interfaces.Infrastructure.Repositories;
using LoanManagementSystem.Domain.Entities;
using System.Data;

namespace LoanManagementSystem.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly IDbConnection _connection;
    private readonly IUnitOfWork _unitOfWork;

    public ClientRepository(IDbConnection connection, IUnitOfWork unitOfWork)
    {
        _connection = connection;
        _unitOfWork = unitOfWork;
    }

    public async Task<Client?> GetByPersonalNumberAsync(string personalNumber)
    {
        const string sql = @"SELECT * FROM Clients WHERE PersonalNumber = @PersonalNumber";

        return await _connection.QueryFirstOrDefaultAsync<Client>(sql, new { PersonalNumber = personalNumber }, _unitOfWork.Transaction);
    }

    public async Task<Guid> CreateAsync(Client client)
    {
        const string sql = @"
            INSERT INTO Clients (Id, FirstName, LastName, PersonalNumber, MonthlyIncome)
            VALUES (@Id, @FirstName, @LastName, @PersonalNumber, @MonthlyIncome);
        ";

        await _connection.ExecuteAsync(sql, client, _unitOfWork.Transaction);
        return client.Id;
    }

    public async Task UpdateAsync(Client client)
    {
        const string sql = @"
            UPDATE Clients 
            SET FirstName = @FirstName, 
                LastName = @LastName, 
                MonthlyIncome = @MonthlyIncome
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, client, _unitOfWork.Transaction);
    }
}
