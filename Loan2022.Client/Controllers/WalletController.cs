using Loan2022.Application.Constants;
using Loan2022.Application.Enums;
using Loan2022.Client.Models;
using Loan2022.ViewModels.WithdrawalRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Loan2022.Client.Controllers;

[Authorize(Roles = Roles.Customer)]
public partial class HomeController
{
    // GET

    public async Task<IActionResult> Wallet()
    {
        var customer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
        ViewBag.Customer = customer;

        if (customer.ContractId.HasValue)
        {
            var contract = await _contractService.GetById(customer.ContractId.Value);
            var signature = await _mediaCustomerService.GetSignatureContractCustomer(customer.Id);
            var month = await _interestService.GetById(contract.InterestId.Value);
            ViewBag.Contract = contract;
            ViewBag.Signature = signature;
            var dieukhoan = await _settingService.GetByName("Dieu_Khoan_Hop_Dong");
            var tencty = await _settingService.GetByName("Ten_Cong_Ty");
            ViewBag.TemplateContract = dieukhoan?.Value;
            ViewBag.CompanyName = tencty?.Value;
            ViewBag.Month = month;
        }
        var history = await _customerWalletHistoryService.GetCustomerWalletHistoryByCustomerId(customer.Id);
        var em = await _employeeService.GetEmployeeByCustomer(customer.Id);
        ViewBag.Employee = em;
        ViewBag.History = history;

        if (customer.BankId.HasValue)
        {
            var bank = await _bankService.GetById(customer.BankId.Value);
            ViewBag.Bank = bank;
        }

        return View("~/Views/Wallet/Index.cshtml");
    }

    public async Task<IActionResult> WithDraw()
    {
        var customer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
        var contract = await _contractService.GetById(customer.ContractId.Value);
        if (contract == null || contract?.Status != ContractStatus.Approved.ToString() || !contract.IsWithdrawMoney)
        {
            return Redirect("/wallet");
        }
        
        ViewBag.Customer = customer;
        ViewBag.Contract = contract;
        return View("~/Views/Wallet/WithDraw.cshtml");
    }

    [Route("api/withdraw-request")]
    [HttpPost]
    public async Task<IActionResult> MakeWithdrawRequest([FromForm]decimal money)
    {
        var customer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
        var contract = await _contractService.GetContractCustomer(customer.Id);
        if (contract == null || !contract.IsWithdrawMoney || contract.Status != ContractStatus.Approved.ToString()) return BadRequest();
        if (customer.TotalMoney < money || money <=0)
        {
            return BadRequest();
        }

        var reqest = new WithdrawalRequestDto()
        {
            CustomerId = customer.Id,
            AmountOfMoney = money,
            Status = WithdrawStatus.Pending.ToString(),
            Name = "Rút tiền"
        };
        try
        {
            var id = await _withdrawalRequestService.Create(reqest);
            TempData["RequestId"] = JsonConvert.SerializeObject(id);
        }
        
        catch (Exception e)
        {
            return BadRequest();
        }

        return Ok();
    }

    public async Task<IActionResult> WithdrawInvoice()
    {
        var requestId = TempData["RequestId"];
        if (requestId == null)
        {
            return Redirect("/wallet");
        }
        var model = new WithdrawInvoiceViewModel();
        var customer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
        model.Customer = customer;
        var request = await _withdrawalRequestService.GetById(JsonConvert.DeserializeObject<long>(requestId.ToString()));
        model.Request = request;
        if (customer.BankId.HasValue)
        {
            var bank = await _bankService.GetById(customer.BankId.Value);
            model.Bank = bank;
        }
        var info = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
        DateTimeOffset localServerTime = DateTimeOffset.Now;
        DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
        model.CreatedAt = localTime.DateTime;
        return View("~/Views/Wallet/WithdrawInvoice.cshtml", model);
    }

}