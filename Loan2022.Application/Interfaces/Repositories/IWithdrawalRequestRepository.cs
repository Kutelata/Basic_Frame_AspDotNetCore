using Loan2022.Domain.Entities;

namespace Loan2022.Application.Interfaces.Repositories;

public interface IWithdrawalRequestRepository
{
    IQueryable<WithdrawalRequest> WithdrawalRequests { get; }

    Task<List<WithdrawalRequest>> GetListAsync();

    Task<WithdrawalRequest> GetByIdAsync(long id);

    Task<long> InsertAsync(WithdrawalRequest data);

    Task UpdateAsync(WithdrawalRequest data);

    Task DeleteAsync(WithdrawalRequest data);
}