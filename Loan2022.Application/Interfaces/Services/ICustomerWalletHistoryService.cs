using Loan2022.Result;
using Loan2022.ViewModels.Bank;

namespace Loan2022.Application.Interfaces.Services;

public interface ICustomerWalletHistoryService
{
    Task<List<CustomerWalletHistoryDto>> GetCustomerWalletHistoryByCustomerId(long id);
    Task<bool> Create(CustomerWalletHistoryDto input);
    Task Update(CustomerWalletHistoryDto input);
    Task<CustomerWalletHistoryDto> GetById(long id);
    Task Delete(long id);
}