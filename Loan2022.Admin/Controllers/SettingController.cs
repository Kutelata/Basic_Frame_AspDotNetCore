using Loan2022.Application.Constants;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Employee;
using Loan2022.ViewModels.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan2022.Admin.Controllers;
[Authorize(Roles = Roles.Admin)]
public class SettingController : Controller
{
    private readonly ISettingService _settingService;

    public SettingController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult CreateOrUpdateSettingView(long? id)
    {
        @ViewBag.id = id;
        return View("~/Views/Setting/CreateOrUpdateSettingView.cshtml");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task CreateOrUpdate([FromBody] SettingDto input)
    {
        await _settingService.CreateOrUpdate(input);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<PaginatedResult<SettingDto>> GetSettings([FromBody] SettingInputDto input)
    {
        var data = await _settingService.GetAll(input);
        return data;
    }

    public async Task DeleteSetting(long id)
    {
        await _settingService.Delete(id);
    }
    
    public async Task<SettingDto> GetSetting(long id)
    {
        var data = await _settingService.GetById(id);
        return data;
    }
    
    public async Task<SettingDto> GetSettingByName(string input)
    {
        var data = await _settingService.GetByName(input);
        return data;
    }

    public async Task<SettingDto> GetSettingForDetail(long id)
    {
        var data = await _settingService.GetById(id);
        @ViewBag.id = id;
        return data;
    }
}