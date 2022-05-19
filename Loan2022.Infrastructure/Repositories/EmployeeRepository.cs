using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class EmployeeRepository: IEmployeeRepository
{
    private readonly IRepositoryAsync<Employee> _repository;

    public EmployeeRepository(IRepositoryAsync<Employee> repository)
    {
        _repository = repository;
    }

    public IQueryable<Employee> Employees => _repository.Entities;
    public async Task<List<Employee>> GetListAsync()
    {
     var query =  await _repository.GetAllAsync();
     return query.ToList();
    }

    public async Task<Employee> GetByIdAsync(long employeeId)
    {
        return await _repository.Entities.Where(p => p.Id == employeeId).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(Employee employee)
    {
        await _repository.AddAsync(employee);
        return employee.Id;
    }

    public async Task UpdateAsync(Employee employee)
    {
        await _repository.UpdateAsync(employee);
    }

    public async Task DeleteAsync(Employee employee)
    {
        await _repository.DeleteAsync(employee);
    }
}