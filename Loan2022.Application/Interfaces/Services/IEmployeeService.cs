using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Customer;
using Loan2022.ViewModels.Employee;

namespace Loan2022.Application.Interfaces.Services;

public interface IEmployeeService
{
    Task<PaginatedResult<EmployeeListDto>> GetAll(EmployeeInputDto input);
    Task CreateOrUpdate(CreateOrEditEmployeeDto input);
    Task<CreateOrEditEmployeeDto> GetById(long id);
    Task Delete(long id);
    Task UpdateStatus(long id, string status);
    Task<long> GetRandom();
    Task<long> GetRamDomEmployee();
    Task<CreateOrEditEmployeeDto> GetEmployeeByCustomer(long customerId);
}