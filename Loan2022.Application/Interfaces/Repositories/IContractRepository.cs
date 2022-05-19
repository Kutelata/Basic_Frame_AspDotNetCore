using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface IContractRepository
{
    IQueryable<Contract> Contracts { get; }

    Task<List<Contract>> GetListAsync();

    Task<Contract> GetByIdAsync(long contractId);

    Task<long> InsertAsync(Contract contract);

    Task UpdateAsync(Contract contract);

    Task DeleteAsync(Contract contract);
}