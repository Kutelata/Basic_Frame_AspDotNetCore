using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Loan2022.Infrastructure.CacheRepository.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;

namespace Loan2022.Infrastructure.CacheRepository.CacheRepositories;

public class InterestCacheRepository:IInterestCacheRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly IInterestRepository _interestRepository;
    public InterestCacheRepository(IDistributedCache distributedCache, IInterestRepository interestRepository)
    {
        _distributedCache = distributedCache;
        _interestRepository = interestRepository;
    }
    public async Task<Interest> GetByIdAsync(long id)
    {
        string cacheKey = InterestCacheKeys.GetKey(id);
        var interest = await _distributedCache.GetAsync<Interest>(cacheKey);
        if (interest == null)
        {
            interest = await _interestRepository.GetByIdAsync(id);
            await _distributedCache.SetAsync(cacheKey, interest);
        }
        return interest;
    }

    public async Task DeleteByIdAsync(long bankId)
    {
        string cacheKey = BankCacheKeys.GetKey(bankId);
        await _distributedCache.RemoveAsync(cacheKey);
    }
    public async Task UpdateAsync(Interest interest)
    {
        string cacheKey = BankCacheKeys.GetKey(interest.Id);
        await _distributedCache.SetAsync(cacheKey, interest);
    }
}