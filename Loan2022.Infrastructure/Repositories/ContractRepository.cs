using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class ContractRepository:IContractRepository
{
    private readonly IRepositoryAsync<Contract> _repository;

    public ContractRepository(IRepositoryAsync<Contract> repository)
    {
        _repository = repository;
    }

    public IQueryable<Contract> Contracts => _repository.Entities;
    public async Task<List<Contract>> GetListAsync()
    {
        var query =  await _repository.GetAllAsync();
        return query.ToList();
    }

    public async Task<Contract> GetByIdAsync(long contractId)
    {
        return await _repository.Entities.Where(p => p.Id == contractId).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(Contract contract)
    {
        await _repository.AddAsync(contract);
        return contract.Id;
    }

    public async Task UpdateAsync(Contract contract)
    {
        await _repository.UpdateAsync(contract);
    }

    public async Task DeleteAsync(Contract contract)
    {
        await _repository.DeleteAsync(contract);
    }
}