using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Contract;
using Loan2022.ViewModels.Customer;
using Loan2022.ViewModels.MediasCustomer;

namespace Loan2022.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<PaginatedResult<CustomerListDto>> GetAll(CustomerInputDto input);
    Task<CustomersForDashboardDto> GetCustomerForDashboard();
    Task CreateOrUpdate(CreateOrEditCustomerDto input);
    Task<CreateOrEditCustomerDto> GetById(long id);
    Task<GetCustomerForDetail> GetForDetail(long id);
    Task Delete(long id);
    Task<bool> CheckVerifiedByCurrentUser();
    Task<GetCustomerForDetail> GetCustomerByUser(string id);
    Task<bool> CheckVerified(long id);
    Task<bool> CheckIdentityExist(string number);
    Task VerifiedCustomer(long customerId);
    Task<List<CustomerExcelDto>> GetCustomerExcel(CustomerExcelInputDto input);
    Task<ContractDto> CreateCustomerContract(MediaDto file, long customerId, long interestId, decimal money);
}