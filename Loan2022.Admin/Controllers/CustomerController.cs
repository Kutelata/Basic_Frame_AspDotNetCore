using System.Globalization;
using ClosedXML.Excel;
using Loan2022.Application.Constants;
using Loan2022.Application.Enums;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Result;
using Loan2022.ViewModels.Account;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Contract;
using Loan2022.ViewModels.Customer;
using Loan2022.ViewModels.MediasCustomer;
using Loan2022.ViewModels.WithdrawalRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Loan2022.Admin.Controllers;

[Authorize(Roles = Roles.Admin)]
public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly IUserService _userService;
    private readonly IMediaCustomerService _mediaCustomerService;
    private readonly ICustomerWalletHistoryService _customerWalletHistoryService;
    private readonly IContractService _contractService;
    private readonly IWithdrawalRequestService _withdrawalRequestService;

    public CustomerController
    (
        ICustomerService customerService,
        IUserService userService,
        IMediaCustomerService mediaCustomerService,
        ICustomerWalletHistoryService customerWalletHistoryService,
        IContractService contractService,
        IWithdrawalRequestService withdrawalRequestService

    )
    {
        _customerService = customerService;
        _userService = userService;
        _mediaCustomerService = mediaCustomerService;
        _customerWalletHistoryService = customerWalletHistoryService;
        _contractService = contractService;
        _withdrawalRequestService = withdrawalRequestService;
    }

    public async Task<IActionResult> Customers(string filter, int pageNumber, int pageSize)
    {
        var input = new CustomerInputDto()
        {
            Filter = filter,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var customers = await _customerService.GetAll(input);
        var customersForDashboard = await _customerService.GetCustomerForDashboard();
        ViewBag.Customers = customers;
        ViewBag.CustomersForDashboard = customersForDashboard;
        return View("~/Views/Customer/Index.cshtml");
    }

    [HttpPost]
    public async Task<PaginatedResult<CustomerListDto>> GetCustomers([FromBody] CustomerInputDto input)
    {
        var customers = await _customerService.GetAll(input);
        return customers;
    }
    public async Task<List<CustomerWalletHistoryDto>> GetCustomerWalletHistoryByCustomerId(long id)
    {
        var histories = await _customerWalletHistoryService.GetCustomerWalletHistoryByCustomerId(id);
        return histories;
    }
    
    public async Task<Decimal> GetTotalMoneyCustomer(long id)
    {
        var customer = await _customerService.GetById(id);
        return customer.TotalMoney;
    }
    [HttpPost] 
    public async Task<bool> AddCustomerWalletHistory([FromBody] CustomerWalletHistoryDto input)
    {
      var result =  await _customerWalletHistoryService.Create(input);
      return result;
    }

    public async Task<CustomersForDashboardDto> GetCustomersForDashboard()
    {
        var customersForDashboard = await _customerService.GetCustomerForDashboard();
        return customersForDashboard;
    }   
    public async Task VerifiedCustomer(long id)
    {
         await _customerService.VerifiedCustomer(id);
    }

    public async Task DeleteCustomer(long id)
    {
        await _customerService.Delete(id);
    }

    public async Task<IActionResult> GetCustomer(long id)
    {
        @ViewBag.id = id;
        return View("~/Views/Customer/CustomerDetail.cshtml");
    }

    public async Task<GetCustomerForDetail> GetCustomerForDetail(long id)
    {
        var customer = await _customerService.GetForDetail(id);
        @ViewBag.id = id;
        return customer;
    }
    
    
    public async Task<IActionResult> ChangePasswordView(string id)
    {
        @ViewBag.id = id;
        return View("/Views/Customer/ChangePasswordCustomer.cshtml");
    }
    public async Task<IActionResult> UpdateCustomerView(string id)
    {
        @ViewBag.id = id;
        return View("/Views/Customer/UpdateCustomer.cshtml");
    }


    public async Task<List<MediaDto>> GetMediasCustomer(long id)
    {
        var medias = await _mediaCustomerService.GetMediasCustomer(id);
        return medias;
    }  
    public async Task<ContractDto> GetContractCustomer(long id)
    {
        var contract = await _contractService.GetContractCustomer(id);
        return contract;
    }
    
    public async Task<string> GetSignatureContract(long id)
    {
       var result = await _mediaCustomerService.GetSignatureContractCustomer(id);
       return result;
    } 
    public async Task<List<WithdrawalRequestDto>> GetWithdrawalRequestCustomer(long id)
    {
       var result = await _withdrawalRequestService.GetAll(id);
       return result;
    }
    
    [HttpPost]
    public async Task<bool> ChangePassword([FromBody]ChangePasswordDto input)
    {
        var result = await _userService.ChangePassword(input);
        return result;
    }
    
    [HttpPost]
    public async Task UpdateCustomer([FromBody]CreateOrEditCustomerDto input)
    {
        await _customerService.CreateOrUpdate(input);
    }

    public async Task<IActionResult> Excel(DateTime startDate, DateTime endDate )
    {
        var input = new CustomerExcelInputDto()
        {
            StartDate = startDate,
            EndDate = endDate,
        };
        var customers = await _customerService.GetCustomerExcel(input);
        var fileName = "KhachHang.xlsx";
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Khach_Hang");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Họ và tên";
            worksheet.Cell(currentRow, 3).Value = "Số điện thoại";
            worksheet.Cell(currentRow, 4).Value = "Số tiền vay";
            worksheet.Cell(currentRow, 5).Value = "Thời gian vay";
            worksheet.Cell(currentRow, 6).Value = "Trạng thái";
            worksheet.Cell(currentRow, 7).Value = "Ngày tạo";
            foreach (var customer in customers)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = currentRow-1;
                worksheet.Cell(currentRow, 2).Value = customer.FullName;
                worksheet.Cell(currentRow, 3).Value = customer.PhoneNumber;
                worksheet.Cell(currentRow, 4).Value = customer.AmountOfMoney;
                worksheet.Cell(currentRow, 5).Value = customer.InterestName;
                worksheet.Cell(currentRow, 6).Value = GetStatusName(customer.Status);
                worksheet.Cell(currentRow, 7).Value = $"{customer.CreatedOn.Day}/{customer.CreatedOn.Month}/{customer.CreatedOn.Year}";
            }

            for (int i = 1; i < 8; i++)
            {
                worksheet.Column(i).Width = 25;
            }
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, contentType, fileName);
            }
        }
    }
    private string GetStatusName(string input)
    {
        switch (input)
        {
            case  "Pending":
                return "Chờ duyệt";  
            case  "Approved":
                return "Đã duyệt"; 
            case  "Finished":
                return "Đã hoàn thành";   
            case  "Reject":
                return "Bị từ chối";
            default:
                return "";
        }
    }
}