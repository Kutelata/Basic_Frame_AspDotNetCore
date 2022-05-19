using Loan2022.Application.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan2022.Client.Controllers;

public partial class HomeController
{
    // GET
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> Profile()
    {
        var customer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
        var avatar = await _mediaCustomerService.GetAvatarCustomer(customer.Id);
        var contract = await _contractService.GetContractCustomer(customer.Id);
        var em = await _employeeService.GetEmployeeByCustomer(customer.Id);
        var history = await _customerWalletHistoryService.GetCustomerWalletHistoryByCustomerId(customer.Id);
        if (customer.BankId.HasValue)
        {
            var bank = await _bankService.GetById(customer.BankId.Value);
            ViewBag.BankName = bank.BankName;
        }

        if (contract != null && contract.InterestId.HasValue)
        {
            var inter  =await _interestService.GetById(contract.InterestId.Value);
            ViewBag.Interest = inter;
        }
        ViewBag.Avatar = $"{_appSettings.MediaDomain}/{avatar}";
        ViewBag.Customer = customer;
        ViewBag.Contract = contract;
        ViewBag.Employee = em;
        ViewBag.History = history;
        return View("~/Views/Profile/Index.cshtml");
    }
}