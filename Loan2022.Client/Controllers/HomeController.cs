using System.Diagnostics;
using AutoMapper;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Application.Interfaces.Shared;
using Loan2022.Application.Settings;
using Microsoft.AspNetCore.Mvc;
using Loan2022.Client.Models;
using Loan2022.Infrastructure.Identity.Models;
using Loan2022.ViewModels.Bank;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Loan2022.Client.Controllers;

public partial class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ICustomerService _customerService;
    private readonly IContractService _contractService;
    private readonly IAuthenticatedUserService _authenticatedUser;
    private readonly IFileService _fileService;
    private readonly AppSettings _appSettings;
    private readonly IMediaCustomerService _mediaCustomerService;
    private readonly IBankService _bankService;
    private readonly IMapper _mapper;
    private readonly IInterestService _interestService;
    private readonly IEmployeeService _employeeService;
    private readonly ISettingService _settingService;
    private readonly IWithdrawalRequestService _withdrawalRequestService;
    private readonly ICustomerWalletHistoryService _customerWalletHistoryService;

    public HomeController(
        ILogger<HomeController> logger,
        SignInManager<ApplicationUser> signInManager,
        ICustomerService customerService,
        IContractService contractService,
        IAuthenticatedUserService authenticatedUser,
        IFileService fileService,
        IOptions<AppSettings> appSettings,
        IBankService bankService,
        IMapper mapper,
        IInterestService interestService,
        IEmployeeService employeeService,
        IMediaCustomerService mediaCustomerService,
        ISettingService settingService,
        IWithdrawalRequestService withdrawalRequestService,
        ICustomerWalletHistoryService customerWalletHistoryService){
        _logger = logger;
        _signInManager = signInManager;
        _customerService = customerService;
        _contractService = contractService;
        _authenticatedUser = authenticatedUser;
        _fileService = fileService;
        _appSettings = appSettings.Value;
        _bankService = bankService;
        _mapper = mapper;
        _interestService = interestService;
        _employeeService = employeeService;
        _mediaCustomerService = mediaCustomerService;
        _settingService = settingService;
        _withdrawalRequestService = withdrawalRequestService;
        _customerWalletHistoryService = customerWalletHistoryService;
    }
    
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            return RedirectToAction("Borrow");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}