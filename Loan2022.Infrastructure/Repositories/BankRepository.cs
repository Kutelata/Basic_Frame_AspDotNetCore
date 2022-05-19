using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class BankRepository: IBankRepository
{
    private readonly IRepositoryAsync<Bank> _repository;

    public BankRepository(IRepositoryAsync<Bank> repository)
    {
        _repository = repository;
    }

    public IQueryable<Bank> Banks => _repository.Entities;
    public async Task<List<Bank>> GetListAsync()
    {
     var query =  await _repository.GetAllAsync();
     return query.ToList();
    }

    public async Task<Bank> GetByIdAsync(long bankId)
    {
        return await _repository.Entities.Where(p => p.Id == bankId).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(Bank bank)
    {
        await _repository.AddAsync(bank);
        return bank.Id;
    }

    public async Task UpdateAsync(Bank bank)
    {
        await _repository.UpdateAsync(bank);
    }

    public async Task DeleteAsync(Bank bank)
    {
        await _repository.DeleteAsync(bank);
    }
}