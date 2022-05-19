using Loan2022.ViewModels.Account;
using Loan2022.ViewModels.Customer;
using Loan2022.ViewModels.Employee;

namespace Loan2022.Application.Interfaces.Services;

public interface IUserService
{
    Task<bool> CreateUserCustomer(RegisterInput input, CreateOrEditCustomerDto customerDto);
    Task<bool> CreateUserEmployee(RegisterInput input, CreateOrEditEmployeeDto employeeDto);
    Task<bool> ChangePassword(ChangePasswordDto input);
}