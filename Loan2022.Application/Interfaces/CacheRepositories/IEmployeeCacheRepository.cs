using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.CacheRepositories;

public interface IEmployeeCacheRepository
{
    Task<Employee> GetByIdAsync(long employeeId);
    Task DeleteByIdAsync(long employeeId);
    Task UpdateAsync(Employee employee);
}