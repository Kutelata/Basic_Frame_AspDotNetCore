using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface ISettingRepository
{
    IQueryable<Setting> Settings { get; }

    Task<List<Setting>> GetListAsync();

    Task<Setting> GetByIdAsync(long id);

    Task<long> InsertAsync(Setting data);

    Task UpdateAsync(Setting bank);

    Task DeleteAsync(Setting bank);
}