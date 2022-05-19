using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Loan2022.Infrastructure.CacheRepository.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;

namespace Loan2022.Infrastructure.CacheRepository.CacheRepositories;

public class CustomerCacheRepository:ICustomerCacheRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly ICustomerRepository _customerRepository;
    public CustomerCacheRepository(IDistributedCache distributedCache, ICustomerRepository customerRepository)
    {
        _distributedCache = distributedCache;
        _customerRepository = customerRepository;
    }
    public async Task<Customer> GetByIdAsync(long customerId)
    {
        string cacheKey = CustomerCacheKeys.GetKey(customerId);
        var customer = await _distributedCache.GetAsync<Customer>(cacheKey);
        if (customer == null)
        {
            customer = await _customerRepository.GetByIdAsync(customerId);
            await _distributedCache.SetAsync(cacheKey, customer);
        }
        return customer;
    }
    public async Task DeleteByIdAsync(long customerId)
    {
        string cacheKey = CustomerCacheKeys.GetKey(customerId);
        await _distributedCache.RemoveAsync(cacheKey);
    }
    public async Task UpdateAsync(Customer customer)
    {
        string cacheKey = CustomerCacheKeys.GetKey(customer.Id);
        await _distributedCache.SetAsync(cacheKey, customer);
    }
}