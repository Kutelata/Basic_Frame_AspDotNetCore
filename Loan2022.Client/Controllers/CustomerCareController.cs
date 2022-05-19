using Loan2022.Application.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan2022.Client.Controllers;

public partial class HomeController
{
    // GET
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> CustomerCare()
    {
        var cusomer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
        var em = await _employeeService.GetEmployeeByCustomer(cusomer.Id);
        var contract = await _contractService.GetContractCustomer(cusomer.Id);
        ViewBag.Employee = em;
        ViewBag.Contract = contract;
        return View("~/Views/CustomerCare/Index.cshtml");
    }
}