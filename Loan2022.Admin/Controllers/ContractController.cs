using Loan2022.Application.Constants;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Contract;
using Loan2022.ViewModels.Interest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan2022.Admin.Controllers;

[Authorize(Roles = Roles.Admin)]
public class ContractController : Controller
{
    private readonly IContractService _contractService;
    private readonly ICustomerWalletHistoryService _customerWalletHistoryService;

    public ContractController(
        IContractService contractService,
        ICustomerWalletHistoryService customerWalletHistoryService
    )
    {
        _contractService = contractService;
        _customerWalletHistoryService = customerWalletHistoryService;
    }

    [HttpPost]
    public async Task UpdateContract([FromBody] ContractDto input)
    {
        await _contractService.CreateOrUpdate(input);
    }
    [HttpPost]
    public async Task ApproveContract([FromBody] ApproveContractDto input)
    {
        await _contractService.ApproveContract(input);
        var inputWallet = new CustomerWalletHistoryDto()
        {
            Amount = input.AmountOfMoney,
            Description = "Hồ sơ vay được chấp thuận",
            Name = "Cộng tiền",
            CustomerId = input.CustomerId,
            Type = "Plus"
        };
        await _customerWalletHistoryService.Create(inputWallet);
    }
}