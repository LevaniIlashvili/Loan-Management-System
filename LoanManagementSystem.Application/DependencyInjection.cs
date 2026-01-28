using FluentValidation;
using LoanManagementSystem.Application.Interfaces.Services;
using LoanManagementSystem.Application.Services;
using LoanManagementSystem.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace LoanManagementSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ILoanService, LoanService>();

        services.AddValidatorsFromAssemblyContaining<SubmitLoanApplicationDtoValidator>();

        return services;
    }
}
