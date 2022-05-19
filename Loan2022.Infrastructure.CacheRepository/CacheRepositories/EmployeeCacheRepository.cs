using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Loan2022.Infrastructure.CacheRepository.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;

namespace Loan2022.Infrastructure.CacheRepository.CacheRepositories;

public class EmployeeCacheRepository : IEmployeeCacheRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly IEmployeeRepository _customerRepository;

    public EmployeeCacheRepository(IDistributedCache distributedCache, IEmployeeRepository customerRepository)
    {
        _distributedCache = distributedCache;
        _customerRepository = customerRepository;
    }

    public async Task<Employee> GetByIdAsync(long employeeId)
    {
        string cacheKey = EmployeeCacheKeys.GetKey(employeeId);
        var employee = await _distributedCache.GetAsync<Employee>(cacheKey);
        if (employee == null)
        {
            employee = await _customerRepository.GetByIdAsync(employeeId);
            await _distributedCache.SetAsync(cacheKey, employee);
        }

        return employee;
    }
    public async Task DeleteByIdAsync(long employeeId)
    {
        string cacheKey = EmployeeCacheKeys.GetKey(employeeId);
        await _distributedCache.RemoveAsync(cacheKey);
    }
    public async Task UpdateAsync(Employee employee)
    {
        string cacheKey = EmployeeCacheKeys.GetKey(employee.Id);
        await _distributedCache.SetAsync(cacheKey, employee);
    }
}