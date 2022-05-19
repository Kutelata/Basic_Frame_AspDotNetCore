using Loan2022.Infrastructure.Identity.Models;
using Loan2022.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Loan2022.Admin.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    // GET
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View("~/Views/Login/Index.cshtml");
    }
    [AllowAnonymous]
    [HttpPost]
    [Route("admin/login")]
    public async  Task<IActionResult> Login([FromBody] LoginInput input)
    {
        var login = await _signInManager.PasswordSignInAsync(input.Username, input.Password, true, false);
        bool result = login.Succeeded;
        return Ok(result);
    }
    [Route("account/signout")]
    public async  Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }
}