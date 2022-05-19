using Loan2022.Application.Constants;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Result;
using Loan2022.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan2022.Admin.Controllers;
[Authorize(Roles = Roles.Admin)]
public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IUserService _userService;

    public EmployeeController(
        IEmployeeService employeeService,
        IUserService userService
    )
    {
        _employeeService = employeeService;
        _userService = userService;
    }

    public ActionResult Employees()
    {
        return View("~/Views/Employee/Index.cshtml");
    }

    public IActionResult CreateOrUpdateEmployeeView(long? id)
    {
        @ViewBag.id = id;
        return View("~/Views/Employee/CreateOrUpdate.cshtml");
    }

    [HttpPost]
    public async Task CreateOrUpdate([FromBody] CreateOrEditEmployeeDto input)
    {
        await _employeeService.CreateOrUpdate(input);
        // if (input.Id > 0)
        // {
        //     await _employeeService.CreateOrUpdate(input);
        // }
        // else
        // {
        //     var userEmployee = new RegisterInput()
        //     {
        //         Password = "It@123456",
        //         Username = input.PhoneNumber
        //     };
        //     await _userService.CreateUserEmployee(userEmployee, input);
        // }
    }
    
    [HttpPost]
    public async Task<PaginatedResult<EmployeeListDto>> GetEmployees([FromBody] EmployeeInputDto input)
    {
        var employees = await _employeeService.GetAll(input);
        return employees;
    } 

    public async Task DeleteEmployee(long id)
    {
        await _employeeService.Delete(id);
    }

    public async Task UpdateStatusEmployee(long id, string status)
    {
        await _employeeService.UpdateStatus(id, status);
    }

    public async Task<CreateOrEditEmployeeDto> GetEmployee(long id)
    {
        var employee = await _employeeService.GetById(id);
        return employee;
    }

    public async Task<CreateOrEditEmployeeDto> GetCustomerForDetail(long id)
    {
        var employee = await _employeeService.GetById(id);
        @ViewBag.id = id;
        return employee;
    }
}