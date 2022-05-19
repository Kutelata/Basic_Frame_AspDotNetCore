using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface ICustomerCareRepository
{
    IQueryable<CustomerCare> CustomerCares { get; }

    Task<List<CustomerCare>> GetListAsync();

    Task<CustomerCare> GetByIdAsync(long customerCareId);

    Task<long> InsertAsync(CustomerCare customerCare);

    Task UpdateAsync(CustomerCare customerCare);

    Task DeleteAsync(CustomerCare customerCare);
}