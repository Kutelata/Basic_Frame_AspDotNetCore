using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Loan2022.Admin.Models;
using Loan2022.Application.Constants;
using Loan2022.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Loan2022.Admin.Controllers;
[Authorize(Roles = Roles.Admin)]
public partial class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public HomeController(
        ILogger<HomeController> logger,
        SignInManager<ApplicationUser> signInManager
        )
    {
        _logger = logger;
        _signInManager = signInManager;
    }
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Index()
    {
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