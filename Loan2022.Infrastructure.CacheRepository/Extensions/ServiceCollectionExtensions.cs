using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Infrastructure.CacheRepository.CacheRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace Loan2022.Infrastructure.CacheRepository.Extensions;

public static class ServiceCollectionExtensions
{

    public static void AddCacheRepositories(this IServiceCollection services)
    {
        #region Repositories
        services.AddTransient<IBankCacheRepository, BankCacheRepository>();
        services.AddTransient<IContractCacheRepository, ContractCacheRepository>();
        services.AddTransient<ICustomerCacheRepository, CustomerCacheRepository>();
        services.AddTransient<IEmployeeCacheRepository, EmployeeCacheRepository>();
        services.AddTransient<IInterestCacheRepository, InterestCacheRepository>();
        #endregion Repositories
    }
}