using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class InterestRepository: IInterestRepository
{
    private readonly IRepositoryAsync<Interest> _repository;

    public InterestRepository(IRepositoryAsync<Interest> repository)
    {
        _repository = repository;
    }

    public IQueryable<Interest> Interests => _repository.Entities;
    public async Task<List<Interest>> GetListAsync()
    {
     var query =  await _repository.GetAllAsync();
     return query.ToList();
    }

    public async Task<Interest> GetByIdAsync(long id)
    {
        return await _repository.Entities.Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(Interest interest)
    {
        await _repository.AddAsync(interest);
        return interest.Id;
    }

    public async Task UpdateAsync(Interest interest)
    {
        await _repository.UpdateAsync(interest);
    }

    public async Task DeleteAsync(Interest interest)
    {
        await _repository.DeleteAsync(interest);
    }
}