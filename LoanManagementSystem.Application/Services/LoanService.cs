using FluentValidation;
using LoanManagementSystem.Application.Dtos;
using LoanManagementSystem.Application.Exceptions;
using LoanManagementSystem.Application.Interfaces.Infrastructure;
using LoanManagementSystem.Application.Interfaces.Infrastructure.Repositories;
using LoanManagementSystem.Application.Interfaces.Services;
using LoanManagementSystem.Domain.Entities;

namespace LoanManagementSystem.Application.Services;

public class LoanService : ILoanService
{
    private readonly IClientRepository _clientRepository;
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly IRepaymentScheduleRepository _repaymentScheduleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SubmitLoanApplicationDto> _validator;

    public LoanService(
        IClientRepository clientRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IRepaymentScheduleRepository repaymentScheduleRepository,
        IUnitOfWork unitOfWork,
        IValidator<SubmitLoanApplicationDto> validator)
    {
        _clientRepository = clientRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _repaymentScheduleRepository = repaymentScheduleRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Guid> CreateLoanApplicationAsync(SubmitLoanApplicationDto request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var client = await _clientRepository.GetByPersonalNumberAsync(request.PersonalNumber);

        if (client == null)
        {
            client = Client.Create(
                request.FirstName,
                request.LastName,
                request.PersonalNumber,
                request.MonthlyIncome);

            await _clientRepository.CreateAsync(client);
        }
        else
        {
            client.UpdateIncome(request.MonthlyIncome);
            await _clientRepository.UpdateAsync(client);
        }

        var application = LoanApplication.Create(client.Id, request.Amount, request.TermInMonths, client.MonthlyIncome);

        return await _loanApplicationRepository.CreateAsync(application);
    }

    public async Task<List<LoanApplicationDto>> GetLoanApplicationsWithClientsAsync()
    {
        var loanApplications = await _loanApplicationRepository.GetAllWithClientsAsync();

        return loanApplications.ToList();
    }

    public async Task ApproveLoanAsync(Guid id)
    {
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var loan = await _loanApplicationRepository.GetByIdAsync(id);

            if (loan == null)
            {
                throw new NotFoundException("Loan not found");
            }

            var schedules = loan.Approve();

            await _loanApplicationRepository.UpdateAsync(loan);

            await _repaymentScheduleRepository.CreateBatchAsync(schedules);
        });
    }

    public async Task RejectLoanAsync(Guid id)
    {
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var loan = await _loanApplicationRepository.GetByIdAsync(id);

            if (loan == null)
            {
                throw new NotFoundException("Loan not found");
            }

            loan.Reject();

            await _loanApplicationRepository.UpdateAsync(loan);
        });
    }

    public async Task<LoanApplicationDetailsDto> GetLoanApplicationDetailsAsync(Guid id)
    {
        var loanDetails = await _loanApplicationRepository.GetLoanApplicationDetailsAsync(id);

        if (loanDetails == null)
        {
            throw new NotFoundException("Loan not found");
        }

        return loanDetails;
    }
}
