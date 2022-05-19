using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.CacheRepositories;

public interface IInterestCacheRepository
{
    Task<Interest> GetByIdAsync(long id);
    Task DeleteByIdAsync(long id);
    Task UpdateAsync(Interest id);
}