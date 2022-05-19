using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface IMediaCustomerRepository
{
    IQueryable<MediaCustomer> MediaCustomers { get; }

    Task<List<MediaCustomer>> GetListAsync();

    Task<MediaCustomer> GetByIdAsync(long mediaCustomerId);

    Task<long> InsertAsync(MediaCustomer mediaCustomer);

    Task UpdateAsync(MediaCustomer mediaCustomer);

    Task DeleteAsync(MediaCustomer mediaCustomer);
}