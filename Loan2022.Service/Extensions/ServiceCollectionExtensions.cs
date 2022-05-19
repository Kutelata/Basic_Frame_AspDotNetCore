using Loan2022.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Loan2022.Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<IEmployeeService, EmployeeService>();
        services.AddTransient<IBankService, BankService>();
        services.AddTransient<IContractService, ContractService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IInterestService,InterestService>();
        services.AddTransient<IMediaCustomerService,MediaCustomerService>();
        services.AddTransient<ICustomerWalletHistoryService,CustomerWalletHistoryService>();
        services.AddTransient<ISettingService,SettingService>();
        services.AddTransient<IWithdrawalRequestService,WithdrawalRequestService>();
    }
}