using System.Security.Claims;
using System.Transactions;
using AutoMapper;
using Loan2022.Application.Constants;
using Loan2022.Application.Enums;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Application.Interfaces.Shared;
using Loan2022.Application.Settings;
using Loan2022.Client.Models;
using Loan2022.Infrastructure.DbContexts;
using Loan2022.Infrastructure.Identity.Models;
using Loan2022.ViewModels.Account;
using Loan2022.ViewModels.Customer;
using Loan2022.ViewModels.MediasCustomer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Loan2022.Client.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ICustomerService _customerService;
    private readonly IUserService _userService;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IFileService _fileService;
    private readonly AppSettings _appSettings;
    private readonly IMediaCustomerService _mediaCustomerService;
    private readonly IMapper _mapper;
    private readonly IEmployeeService _employeeService;

    public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager, ICustomerService customerService, IUserService userService,
        IAuthenticatedUserService authenticatedUserService,
        IFileService fileService,
        IOptions<AppSettings> appSettings,
        IMediaCustomerService mediaCustomerService,
        IMapper mapper,
        IEmployeeService employeeService)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _customerService = customerService;
        _userService = userService;
        _authenticatedUserService = authenticatedUserService;
        _fileService = fileService;
        _appSettings = appSettings.Value;
        _mediaCustomerService = mediaCustomerService;
        _mapper = mapper;
        _employeeService = employeeService;
    }

    // GET
    [AllowAnonymous]
    [HttpPost]
    [Route("api/login-by-phone")]
    public async Task<IActionResult> Login([FromBody] LoginInput input)
    {
        var login = await _signInManager.PasswordSignInAsync(input.Username, input.Password, true, false);
        bool result = login.Succeeded;
        return Ok(result);
    }


    [HttpPost]
    [AllowAnonymous]
    [Route("api/check-phone")]
    public async Task<IActionResult> CheckPhoneNumber([FromBody] LoginInput input)
    {
        var user = await _userManager.FindByNameAsync(input.Username);
        var result = false;
        if (user != null)
        {
            result = await _userManager.IsInRoleAsync(user, Roles.Customer);
        }

        return Ok(result);
        //return BadRequest();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("api/register")]
    public async Task<IActionResult> Register([FromBody] RegisterInput input)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(input.Username);
        if (userWithSameUserName != null)
        {
            return BadRequest();
        }

        // var employee =await _employeeService.GetRandom();
        var employee =await _employeeService.GetRamDomEmployee();
        var result = await _userService.CreateUserCustomer(input,
            new CreateOrEditCustomerDto()
                {PhoneNumber = input.Username, Status = CustomerStatus.Unverified.ToString(), EmployeeId = employee});
        if (result)
        {
            var login = await _signInManager.PasswordSignInAsync(input.Username, input.Password, true, false);
            return Ok(login.Succeeded);
        }

        return BadRequest(result);
    }

    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> Verify()
    {
        var customer = await _customerService.GetCustomerByUser(_authenticatedUserService.UserId);
        if (customer.Status == CustomerStatus.Verified.ToString())
        {
            return Redirect("/borrow");
        }

        ViewBag.Customer = customer;
        return View();
    }

    [Authorize(Roles = Roles.Customer)]
    [Route("api/upload-identity-image")]
    [HttpPost]
    public async Task<IActionResult> UploadImages([FromForm] UploadIdentityImageInput input)
    {
        if (input.CheckNull()) return BadRequest();
        var path = _appSettings.UploadImageFolder;
        var frontName =
            $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}-{DateTime.Now.Ticks}{Path.GetExtension(input.IdentityCardFrontFace.FileName)}";
        var backName =
            $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}-{DateTime.Now.Ticks}{Path.GetExtension(input.IdentityCardBackFace.FileName)}";
        var avatarName =
            $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}-{DateTime.Now.Ticks}{Path.GetExtension(input.IdentityAvatar.FileName)}";
        try
        {
            var customer = await _customerService.GetCustomerByUser(_authenticatedUserService.UserId);
            if (customer.CurrentStepVerify != 0)
            {
                return BadRequest();
            }
            var fileMedia = new Dictionary<string, MediaDto>()
            {
                {
                    MediaType.FrontFaceIdentityCard.ToString(),
                    new MediaDto()
                        {Name = frontName, Path = frontName, Extension = input.IdentityCardFrontFace.ContentType}
                },
                {
                    MediaType.BackFaceIdentityCard.ToString(),
                    new MediaDto()
                        {Name = backName, Path = backName, Extension = input.IdentityCardBackFace.ContentType}
                },
                {
                    MediaType.Avatar.ToString(),
                    new MediaDto() {Name = avatarName, Path = avatarName, Extension = input.IdentityAvatar.ContentType}
                }
            };
            await _mediaCustomerService.CreateIdentityCustomerMedia(fileMedia, customer.Id);
            await _fileService.SaveFormFile(input.IdentityCardFrontFace, path, frontName);
            await _fileService.SaveFormFile(input.IdentityCardBackFace, path, backName);
            await _fileService.SaveFormFile(input.IdentityAvatar, path, avatarName);
        }
        catch (Exception e)
        {
            await _fileService.DeleteFile(path, frontName);
            await _fileService.DeleteFile(path, backName);
            await _fileService.DeleteFile(path, avatarName);
            return BadRequest();
        }

        return Ok();
    }


    [Authorize(Roles = Roles.Customer)]
    [Route("api/check-identity")]
    public async Task<IActionResult> CheckIdentityNumber(string number)
    {
        if (string.IsNullOrEmpty(number)) return BadRequest();
        var check = await _customerService.CheckIdentityExist(number);
        return Ok(check);
    }


    [Authorize(Roles = Roles.Customer)]
    [Route("api/update-information")]
    [HttpPost]
    public async Task<IActionResult> UpdateCustomerInformation([FromBody] CustomerInformationInput input)
    {
        try
        {
            input.DateOfBirth = input.DateOfBirth?.AddDays(1);
            var customer = await _customerService.GetCustomerByUser(_authenticatedUserService.UserId);
            if (customer == null || customer?.CurrentStepVerify != 1) return BadRequest();
            var customerUpdate = _mapper.Map<CreateOrEditCustomerDto>(customer);
            _mapper.Map(input, customerUpdate);
            customerUpdate.CurrentStepVerify = 2;
            await _customerService.CreateOrUpdate(customerUpdate);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }

        return Ok();
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }
}