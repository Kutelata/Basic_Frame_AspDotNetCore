using System.Reflection;
using Loan2022.Application.Interfaces.Context;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Infrastructure.DbContexts;
using Loan2022.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Loan2022.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        #region Repositories
        services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IBankRepository, BankRepository>();
        services.AddTransient<IContractRepository, ContractRepository>();
        services.AddTransient<ICustomerCareRepository, CustomerCareRepository>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        services.AddTransient<IMediaCustomerRepository, MediaCustomerRepository>();
        services.AddTransient<IMediaRepository, MediaRepository>();
        services.AddTransient<IInterestRepository, InterestRepository>();
        services.AddTransient<ICustomerWalletHistoryRepository, CustomerWalletHistoryRepository>();
        services.AddTransient<ISettingRepository, SettingRepository>();
        services.AddTransient<IWithdrawalRequestRepository, WithdrawalRequestRepository>();

        #endregion Repositories
    }
}