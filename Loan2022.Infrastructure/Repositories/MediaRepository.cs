using System.Linq;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Repositories;

public class MediaRepository: IMediaRepository
{
    private readonly IRepositoryAsync<Media> _repository;

    public MediaRepository(IRepositoryAsync<Media> repository)
    {
        _repository = repository;
    }

    public IQueryable<Media> Medias => _repository.Entities;
    public async Task<List<Media>> GetListAsync()
    {
     var query =  await _repository.GetAllAsync();
     return query.ToList();
    }

    public async Task<Media> GetByIdAsync(long mediaId)
    {
        return await _repository.Entities.Where(p => p.Id == mediaId).FirstOrDefaultAsync();
    }

    public async Task<long> InsertAsync(Media media)
    {
        await _repository.AddAsync(media);
        return media.Id;
    }

    public async Task UpdateAsync(Media media)
    {
        await _repository.UpdateAsync(media);
    }

    public async Task DeleteAsync(Media media)
    {
        await _repository.DeleteAsync(media);
    }
}