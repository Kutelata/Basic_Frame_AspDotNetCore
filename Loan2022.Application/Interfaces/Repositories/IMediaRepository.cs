using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface IMediaRepository
{
    IQueryable<Media> Medias { get; }

    Task<List<Media>> GetListAsync();

    Task<Media> GetByIdAsync(long mediaId);

    Task<long> InsertAsync(Media media);

    Task UpdateAsync(Media media);

    Task DeleteAsync(Media media);
}