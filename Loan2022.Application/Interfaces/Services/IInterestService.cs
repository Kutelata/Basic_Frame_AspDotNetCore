using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Interest;

namespace Loan2022.Application.Interfaces.Services;

public interface IInterestService
{
    Task<PaginatedResult<InterestDto>> GetAll(InterestInputDto input);
    Task<List<InterestDto>> GetAllInterest();
    Task CreateOrUpdate(InterestDto input);
    Task<InterestDto> GetById(long id);
    Task Delete(long id);
}