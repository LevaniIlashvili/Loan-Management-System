using LoanManagementSystem.Application.Dtos;

namespace LoanManagementSystem.Application.Interfaces.Services;

public interface ILoanService
{
    Task<Guid> CreateLoanApplicationAsync(SubmitLoanApplicationDto request);
    Task<List<LoanApplicationDto>> GetLoanApplicationsWithClientsAsync();
    Task ApproveLoanAsync(Guid id);
    Task RejectLoanAsync(Guid id);
    Task<LoanApplicationDetailsDto> GetLoanApplicationDetailsAsync(Guid id);
}
