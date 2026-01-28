using LoanManagementSystem.Application.Interfaces.Infrastructure;
using LoanManagementSystem.Application.Interfaces.Infrastructure.Repositories;
using LoanManagementSystem.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace LoanManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnection>(provider =>
        {
            var connectionString = configuration.GetConnectionString("Default");
            return new SqlConnection(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
        services.AddScoped<IRepaymentScheduleRepository, RepaymentScheduleRepository>();

        return services;
    }
}
