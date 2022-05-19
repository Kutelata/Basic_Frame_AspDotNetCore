using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface ICustomerWalletHistoryRepository
{
    IQueryable<CustomerWalletHistory> CustomerWalletHistorys { get; }

    Task<List<CustomerWalletHistory>> GetListAsync();

    Task<CustomerWalletHistory> GetByIdAsync(long dataId);

    Task<long> InsertAsync(CustomerWalletHistory data);

    Task UpdateAsync(CustomerWalletHistory data);

    Task DeleteAsync(CustomerWalletHistory data);
}