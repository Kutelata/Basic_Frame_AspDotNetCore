using Loan2022.Application.Constants;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Interest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan2022.Admin.Controllers;
[Authorize(Roles = Roles.Admin)]
public class InterestController : Controller
{
    private readonly IInterestService _interestService;

    public InterestController(IInterestService interestService)
    {
        _interestService = interestService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult CreateOrUpdateInterestView(long? id)
    {
        @ViewBag.id = id;
        return View("~/Views/Interest/CreateOrUpdateInterestView.cshtml");
    }

    [HttpPost]
    public async Task CreateOrUpdate([FromBody] InterestDto input)
    {
        await _interestService.CreateOrUpdate(input);
    }

    [HttpPost]
    public async Task<PaginatedResult<InterestDto>> GetInterests([FromBody] InterestInputDto input)
    {
        var interests = await _interestService.GetAll(input);
        return interests;
    }
    
    public async Task<List<InterestDto>> GetAllInterest()
    {
        var interests = await _interestService.GetAllInterest();
        return interests;
    }


    [HttpDelete]
    public async Task DeleteEmployee(long id)
    {
        await _interestService.Delete(id);
    }
    
    public async Task<InterestDto> GetInterest(long id)
    {
        var interest = await _interestService.GetById(id);
        return interest;
    }

    public async Task<InterestDto> GetInterestForDetail(long id)
    {
        var interest = await _interestService.GetById(id);
        @ViewBag.id = id;
        return interest;
    }
}