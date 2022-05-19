using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class CustomerRepository: ICustomerRepository
{
    private readonly IRepositoryAsync<Customer> _repository;

    public CustomerRepository(IRepositoryAsync<Customer> repository)
    {
        _repository = repository;
    }

    public IQueryable<Customer> Customers => _repository.Entities;
    public async Task<List<Customer>> GetListAsync()
    {
     var query =  await _repository.GetAllAsync();
     return query.ToList();
    }

    public async Task<Customer> GetByIdAsync(long customerId)
    {
        return await _repository.Entities.Where(p => p.Id == customerId).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(Customer customer)
    {
        await _repository.AddAsync(customer);
        return customer.Id;
    }

    public async Task UpdateAsync(Customer customer)
    {
        await _repository.UpdateAsync(customer);
    }

    public async Task DeleteAsync(Customer customer)
    {
        await _repository.DeleteAsync(customer);
    }
}