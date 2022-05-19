using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
    IQueryable<Employee> Employees { get; }

    Task<List<Employee>> GetListAsync();

    Task<Employee> GetByIdAsync(long employeeId);

    Task<long> InsertAsync(Employee employee);

    Task UpdateAsync(Employee employee);

    Task DeleteAsync(Employee employee);
}