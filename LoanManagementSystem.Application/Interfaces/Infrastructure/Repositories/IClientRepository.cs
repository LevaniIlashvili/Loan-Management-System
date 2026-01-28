using LoanManagementSystem.Domain.Entities;

namespace LoanManagementSystem.Application.Interfaces.Infrastructure.Repositories;

public interface IClientRepository
{
    Task<Client?> GetByPersonalNumberAsync(string personalNumber);
    Task<Guid> CreateAsync(Client client);
    Task UpdateAsync(Client client);
}
