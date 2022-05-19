using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class SettingRepository: ISettingRepository
{
    private readonly IRepositoryAsync<Setting> _repository;

    public SettingRepository(IRepositoryAsync<Setting> repository)
    {
        _repository = repository;
    }

    public IQueryable<Setting> Settings => _repository.Entities;
    public async Task<List<Setting>> GetListAsync()
    {
     var query =  await _repository.GetAllAsync();
     return query.ToList();
    }

    public async Task<Setting> GetByIdAsync(long bankId)
    {
        return await _repository.Entities.Where(p => p.Id == bankId).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(Setting data)
    {
        await _repository.AddAsync(data);
        return data.Id;
    }

    public async Task UpdateAsync(Setting data)
    {
        await _repository.UpdateAsync(data);
    }

    public async Task DeleteAsync(Setting data)
    {
        await _repository.DeleteAsync(data);
    }
}