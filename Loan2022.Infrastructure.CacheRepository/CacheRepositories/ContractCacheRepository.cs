using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Loan2022.Infrastructure.CacheRepository.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;

namespace Loan2022.Infrastructure.CacheRepository.CacheRepositories;

public class ContractCacheRepository:IContractCacheRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly IContractRepository _contractRepository;
    public ContractCacheRepository(IDistributedCache distributedCache, IContractRepository contractRepository)
    {
        _distributedCache = distributedCache;
        _contractRepository = contractRepository;
    }
    public async Task<Contract> GetByIdAsync(long contractId)
    {
        string cacheKey = ContractCacheKeys.GetKey(contractId);
        var contract = await _distributedCache.GetAsync<Contract>(cacheKey);
        if (contract == null)
        {
            contract = await _contractRepository.GetByIdAsync(contractId);
            await _distributedCache.SetAsync(cacheKey, contract);
        }
        return contract;
    }
    public async Task DeleteByIdAsync(long contractId)
    {
        string cacheKey = ContractCacheKeys.GetKey(contractId);
        await _distributedCache.RemoveAsync(cacheKey);
    }
    public async Task UpdateAsync(Contract contract)
    {
        string cacheKey = ContractCacheKeys.GetKey(contract.Id);
        await _distributedCache.SetAsync(cacheKey, contract);
    }
}