using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    IQueryable<Customer> Customers { get; }

    Task<List<Customer>> GetListAsync();

    Task<Customer> GetByIdAsync(long customerId);

    Task<long> InsertAsync(Customer customer);

    Task UpdateAsync(Customer customer);

    Task DeleteAsync(Customer customer);
}