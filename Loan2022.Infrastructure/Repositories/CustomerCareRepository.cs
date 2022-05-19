using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class CustomerCareRepository: ICustomerCareRepository
{
    private readonly IRepositoryAsync<CustomerCare> _repository;

    public CustomerCareRepository(IRepositoryAsync<CustomerCare> repository)
    {
        _repository = repository;
    }

    public IQueryable<CustomerCare> CustomerCares => _repository.Entities;
    public async Task<List<CustomerCare>> GetListAsync()
    {
     var query =  await _repository.GetAllAsync();
     return query.ToList();
    }

    public async Task<CustomerCare> GetByIdAsync(long customerCareId)
    {
        return await _repository.Entities.Where(p => p.Id == customerCareId).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(CustomerCare customerCare)
    {
        await _repository.AddAsync(customerCare);
        return customerCare.Id;
    }

    public async Task UpdateAsync(CustomerCare customerCare)
    {
        await _repository.UpdateAsync(customerCare);
    }

    public async Task DeleteAsync(CustomerCare customerCare)
    {
        await _repository.DeleteAsync(customerCare);
    }
}