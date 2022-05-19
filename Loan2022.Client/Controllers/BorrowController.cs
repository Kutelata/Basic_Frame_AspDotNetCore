using Loan2022.Application.Constants;
using Loan2022.Application.Enums;
using Loan2022.Client.Models;
using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Contract;
using Loan2022.ViewModels.Customer;
using Loan2022.ViewModels.MediasCustomer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Loan2022.Client.Controllers;

public partial class HomeController
{
    // GET
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> Borrow()
    { 
        var cusomer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
        var em = await _employeeService.GetEmployeeByCustomer(cusomer.Id);
        ViewBag.Employee = em;
        return View("~/Views/Borrow/Index.cshtml");
    }

    [Authorize(Roles = Roles.Customer)]
    [Route("api/check-loan")]
    public async Task<IActionResult> CheckLoan()
    {
        try
        {
            var customer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
            if (customer != null)
            {
               var checkVerified = await _customerService.CheckVerified(customer.Id);
               var checkContract = await _contractService.CheckAnyContractNotApprovedByCustomer(customer.Id);
               return Ok(new {Verified = checkVerified, CheckContract = !checkContract});
            }
        }
        catch (Exception e)
        {
            
        }

        return BadRequest();
    }
    
    [HttpPost]
    [Authorize(Roles = Roles.Customer)]
    [Route("api/get-banks")]
    public async Task<IActionResult> GetBanks()
    {
        var banks = await _bankService.GetAll();
        return Ok(banks);
    }
    
    [HttpPost]
    [Authorize(Roles = Roles.Customer)]
    [Route("api/update-bank")]
    public async Task<IActionResult> UpdateBank([FromBody]CustomerBankInformationInput input)
    {
        try
        {
            var customer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
            if (customer == null) return BadRequest();
            var customerUpdate = _mapper.Map<CreateOrEditCustomerDto>(customer);
            _mapper.Map(input, customerUpdate);
            customerUpdate.CurrentStepVerify = 0;
            customerUpdate.Status = CustomerStatus.Verified.ToString();
            await _customerService.CreateOrUpdate(customerUpdate);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
        return Ok();
    }

    public async Task<IActionResult> RegisterContract()
    {
        var customer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
        var isValid = await _contractService.CheckAnyContractNotApprovedByCustomer(customer.Id);
        if (!isValid)
        {
            return Redirect("/borrow");
        }

        var months = await _interestService.GetAllInterest();
        ViewBag.Months = months;
        return View("~/Views/Borrow/RegisterContract.cshtml");
    }

    [HttpPost]
    [Route("api/update-combo")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> UpdateCombo([FromBody]ComboContractInput input)
    {
        if (input.IsNull()) return BadRequest();
        TempData["Input"] = JsonConvert.SerializeObject(input);
        return Ok();
    }

    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> ContractSign()
    {
        var input = TempData["Input"];
        var customer = await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
        // var isValid = await _contractService.CheckAnyContractNotApprovedByCustomer(customer.Id);
        if (customer==null) return Redirect("/borrow");
        if ( customer.Status != CustomerStatus.Verified.ToString() || input == null && customer.ContractId == null)
        {
            return Redirect("/borrow");
        }
        
        if (customer.ContractId.HasValue)
        {
            var contract = await _contractService.GetById(customer.ContractId.Value);
            var month = await _interestService.GetById(contract.InterestId.Value);
            var em = await _employeeService.GetEmployeeByCustomer(customer.Id);
            ViewBag.Employee = em;
            ViewBag.Contract = contract;
            ViewBag.Month = month;
        }

        if (input != null)
        {
            var inp = JsonConvert.DeserializeObject<ComboContractInput>(input.ToString());
            var month = await _interestService.GetById(inp.Month.Value);
            ViewBag.Month = month;
            ViewBag.Money = inp.Money;
            TempData["InputSuccess"] = input;
        }

        return View("~/Views/Borrow/ContractSign.cshtml");
    }

    [HttpPost]
    [Route("api/save-contract")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> SaveContract([FromForm] IFormFile file)
    {
        var path = _appSettings.UploadImageFolder;
        var fileName =
            $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}-{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
        try
        {
            var input = TempData["InputSuccess"];
            if (file == null || input == null) return BadRequest();
            var inp = JsonConvert.DeserializeObject<ComboContractInput>(input.ToString());
            var customer =await _customerService.GetCustomerByUser(_authenticatedUser.UserId);
            if (customer != null)
            {
                await _fileService.SaveFormFile(file, path, fileName);
                var contractDto =  await _customerService.CreateCustomerContract(new MediaDto() {Path = path, Name = fileName, Extension = file.ContentType}, customer.Id,
                    inp.Month.Value, inp.Money.Value);
                var em = await _employeeService.GetEmployeeByCustomer(customer.Id);
                return Ok(new {contract = contractDto, chatId=em?.ChatId});
            }
           
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await _fileService.DeleteFile(path, fileName);
            return BadRequest();
        }
       
        return BadRequest();
    }
}