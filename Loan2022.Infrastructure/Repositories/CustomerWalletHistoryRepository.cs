using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class CustomerWalletHistoryRepository: ICustomerWalletHistoryRepository
{
    private readonly IRepositoryAsync<CustomerWalletHistory> _repository;

    public CustomerWalletHistoryRepository(IRepositoryAsync<CustomerWalletHistory> repository)
    {
        _repository = repository;
    }

    public IQueryable<CustomerWalletHistory> CustomerWalletHistorys => _repository.Entities;
    public async Task<List<CustomerWalletHistory>> GetListAsync()
    {
     var query =  await _repository.GetAllAsync();
     return query.ToList();
    }

    public async Task<CustomerWalletHistory> GetByIdAsync(long dataId)
    {
        return await _repository.Entities.Where(p => p.Id == dataId).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(CustomerWalletHistory data)
    {
        await _repository.AddAsync(data);
        return data.Id;
    }

    public async Task UpdateAsync(CustomerWalletHistory data)
    {
        await _repository.UpdateAsync(data);
    }

    public async Task DeleteAsync(CustomerWalletHistory data)
    {
        await _repository.DeleteAsync(data);
    }
}