using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface IBankRepository
{
    IQueryable<Bank> Banks { get; }

    Task<List<Bank>> GetListAsync();

    Task<Bank> GetByIdAsync(long bankId);

    Task<long> InsertAsync(Bank bank);

    Task UpdateAsync(Bank bank);

    Task DeleteAsync(Bank bank);
}