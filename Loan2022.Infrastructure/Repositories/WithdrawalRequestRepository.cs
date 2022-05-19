using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class WithdrawalRequestRepository: IWithdrawalRequestRepository
{
    private readonly IRepositoryAsync<WithdrawalRequest> _repository;

    public WithdrawalRequestRepository(IRepositoryAsync<WithdrawalRequest> repository)
    {
        _repository = repository;
    }

    public IQueryable<WithdrawalRequest> WithdrawalRequests => _repository.Entities;
    public async Task<List<WithdrawalRequest>> GetListAsync()
    {
     var query =  await _repository.GetAllAsync();
     return query.ToList();
    }

    public async Task<WithdrawalRequest> GetByIdAsync(long bankId)
    {
        return await _repository.Entities.Where(p => p.Id == bankId).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(WithdrawalRequest data)
    {
        await _repository.AddAsync(data);
        return data.Id;
    }

    public async Task UpdateAsync(WithdrawalRequest data)
    {
        await _repository.UpdateAsync(data);
    }

    public async Task DeleteAsync(WithdrawalRequest data)
    {
        await _repository.DeleteAsync(data);
    }
}