using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface IInterestRepository
{
    IQueryable<Interest> Interests { get; }

    Task<List<Interest>> GetListAsync();

    Task<Interest> GetByIdAsync(long id);

    Task<long> InsertAsync(Interest interest);

    Task UpdateAsync(Interest interest);

    Task DeleteAsync(Interest interest);
}