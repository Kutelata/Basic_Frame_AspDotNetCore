using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.CacheRepositories;

public interface ICustomerCacheRepository
{
    Task<Customer> GetByIdAsync(long customerId);
    Task DeleteByIdAsync(long customerId);
    Task UpdateAsync(Customer customer);
}