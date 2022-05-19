using Loan2022.Result;
using Loan2022.ViewModels.Bank;

namespace Loan2022.Application.Interfaces.Services;

public interface IBankService
{
    Task<PaginatedResult<BankDto>> GetAll(BankInputDto input);
    Task<List<BankDto>> GetAllBank();
    Task CreateOrUpdate(BankDto input);
    Task<BankDto> GetById(long id);
    Task Delete(long id);
    Task<List<BankDto>> GetAll();
}