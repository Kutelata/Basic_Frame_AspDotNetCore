using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Contract;

namespace Loan2022.Application.Interfaces.Services;

public interface IContractService
{
    Task<PaginatedResult<ContractDto>> GetAll(ContractInputDto input);
    Task CreateOrUpdate(ContractDto input);
    Task<ContractDto> GetById(long id);
    Task Delete(long id);
    Task<bool> CheckAnyContractNotApprovedByCustomer(long id);
    Task<ContractDto> GetContractCustomer(long customerId);
    Task ApproveContract(ApproveContractDto input);
}