namespace Loan2022.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<int> Commit();
    Task<int> Commit(CancellationToken cancellationToken);
    Task Rollback();
}