using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.CacheRepositories;

public interface IContractCacheRepository
{
    Task<Contract> GetByIdAsync(long contractId);
    Task DeleteByIdAsync(long contractId);
    Task UpdateAsync(Contract contract);

}