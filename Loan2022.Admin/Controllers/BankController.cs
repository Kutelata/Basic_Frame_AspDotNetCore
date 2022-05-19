using Loan2022.Application.Constants;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan2022.Admin.Controllers;
[Authorize(Roles = Roles.Admin)]
public class BankController : Controller
{
    private readonly IBankService _bankService;

    public BankController(IBankService bankService)
    {
        _bankService = bankService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult CreateOrUpdateBankView(long? id)
    {
        @ViewBag.id = id;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task CreateOrUpdate([FromBody] BankDto input)
    {
        await _bankService.CreateOrUpdate(input);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<PaginatedResult<BankDto>> GetBanks([FromBody] BankInputDto input)
    {
        var banks = await _bankService.GetAll(input);
        return banks;
    }
    
    public async Task<List<BankDto>> GetAllBank()
    {
        var banks = await _bankService.GetAllBank();
        return banks;
    }

    public async Task DeleteEmployee(long id)
    {
        await _bankService.Delete(id);
    }
    
    public async Task<BankDto> GetBank(long id)
    {
        var bank = await _bankService.GetById(id);
        return bank;
    }

    public async Task<BankDto> GetBankForDetail(long id)
    {
        var bank = await _bankService.GetById(id);
        @ViewBag.id = id;
        return bank;
    }
}