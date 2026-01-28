using LoanManagementSystem.Application.Dtos;
using LoanManagementSystem.Domain.Entities;
using System.Data;

namespace LoanManagementSystem.Application.Interfaces.Infrastructure.Repositories;

public interface ILoanApplicationRepository
{
    Task<LoanApplication?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(LoanApplication loanApplication);
    Task<IEnumerable<LoanApplicationDto>> GetAllWithClientsAsync();
    Task UpdateAsync(LoanApplication loanApplication);
    Task<LoanApplicationDetailsDto?> GetLoanApplicationDetailsAsync(Guid id);
}
