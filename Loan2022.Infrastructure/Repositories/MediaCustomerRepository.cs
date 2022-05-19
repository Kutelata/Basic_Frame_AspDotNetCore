using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class MediaCustomerRepository: IMediaCustomerRepository
{
    private readonly IRepositoryAsync<MediaCustomer> _repository;

    public MediaCustomerRepository(IRepositoryAsync<MediaCustomer> repository)
    {
        _repository = repository;
    }

    public IQueryable<MediaCustomer> MediaCustomers => _repository.Entities;
    public async Task<List<MediaCustomer>> GetListAsync()
    {
     var query =  await _repository.GetAllAsync();
     return query.ToList();
    }

    public async Task<MediaCustomer> GetByIdAsync(long mediaCustomerId)
    {
        return await _repository.Entities.Where(p => p.Id == mediaCustomerId).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(MediaCustomer mediaCustomer)
    {
        await _repository.AddAsync(mediaCustomer);
        return mediaCustomer.Id;
    }

    public async Task UpdateAsync(MediaCustomer mediaCustomer)
    {
        await _repository.UpdateAsync(mediaCustomer);
    }

    public async Task DeleteAsync(MediaCustomer mediaCustomer)
    {
        await _repository.DeleteAsync(mediaCustomer);
    }
}