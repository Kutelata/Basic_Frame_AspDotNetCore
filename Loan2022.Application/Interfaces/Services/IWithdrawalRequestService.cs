using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.WithdrawalRequest;

namespace Loan2022.Application.Interfaces.Services;

public interface IWithdrawalRequestService
{
    Task<List<WithdrawalRequestDto>> GetAll(long customerId);
    Task CreateOrUpdate(WithdrawalRequestDto input);
    Task<WithdrawalRequestDto> GetById(long id);
    Task Delete(long id);
    Task<long> Create(WithdrawalRequestDto input);
}