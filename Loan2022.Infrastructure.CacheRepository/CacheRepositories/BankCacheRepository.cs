using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Loan2022.Infrastructure.CacheRepository.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;

namespace Loan2022.Infrastructure.CacheRepository.CacheRepositories;

public class BankCacheRepository:IBankCacheRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly IBankRepository _bankRepository;
    public BankCacheRepository(IDistributedCache distributedCache, IBankRepository bankRepository)
    {
        _distributedCache = distributedCache;
        _bankRepository = bankRepository;
    }
    public async Task<Bank> GetByIdAsync(long bankId)
    {
        string cacheKey = BankCacheKeys.GetKey(bankId);
        var bank = await _distributedCache.GetAsync<Bank>(cacheKey);
        if (bank == null)
        {
            bank = await _bankRepository.GetByIdAsync(bankId);
            await _distributedCache.SetAsync(cacheKey, bank);
        }
        return bank;
    }

    public async Task DeleteByIdAsync(long bankId)
    {
        string cacheKey = BankCacheKeys.GetKey(bankId);
        await _distributedCache.RemoveAsync(cacheKey);
    }
    public async Task UpdateAsync(Bank bank)
    {
        string cacheKey = BankCacheKeys.GetKey(bank.Id);
        await _distributedCache.SetAsync(cacheKey, bank);
    }
}