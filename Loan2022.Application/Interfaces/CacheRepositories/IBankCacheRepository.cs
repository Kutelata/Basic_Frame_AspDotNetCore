using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.CacheRepositories;

public interface IBankCacheRepository
{
    Task<Bank> GetByIdAsync(long bankId);
    Task DeleteByIdAsync(long bankId);
    Task UpdateAsync(Bank bank);
}