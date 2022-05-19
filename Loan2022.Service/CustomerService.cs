using System.Data;
using AutoMapper;
using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Domain.Entities;
using Loan2022.Result;
using Loan2022.Service.Extensions;
using Loan2022.ViewModels.Customer;
using System.Linq.Dynamic.Core;
using Loan2022.Application.Enums;
using Loan2022.Application.Interfaces.Shared;
using Loan2022.Infrastructure.DbContexts;
using Loan2022.ViewModels.Contract;
using Loan2022.ViewModels.MediasCustomer;
using Loan2022.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Service;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerCareRepository _customerCareRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;
    private readonly ICustomerCacheRepository _customerCacheRepository;
    private readonly IAuthenticatedUserService _authenticatedUser;
    private readonly IMediaCustomerService _mediaCustomerService;
    private readonly IBankRepository _bankRepository;
    private readonly IMediaRepository _mediaRepository;
    private readonly IMediaCustomerRepository _mediaCustomerRepository;
    private readonly IContractRepository _contractRepository;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerService(
        ICustomerRepository customerRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ICustomerCacheRepository customerCacheRepository,
        ICustomerCareRepository customerCareRepository,
        IEmployeeRepository employeeRepository,
        IAuthenticatedUserService authenticatedUser,
        IMediaCustomerService mediaCustomerService,
        IBankRepository bankRepository,
        IMediaRepository mediaRepository,
        IMediaCustomerRepository mediaCustomerRepository,
        IContractRepository contractRepository,
        ApplicationDbContext applicationDbContext,
        UserManager<ApplicationUser> userManager
    )
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _customerCacheRepository = customerCacheRepository;
        _unitOfWork = unitOfWork;
        _customerCareRepository = customerCareRepository;
        _employeeRepository = employeeRepository;
        _authenticatedUser = authenticatedUser;
        _mediaCustomerService = mediaCustomerService;
        _bankRepository = bankRepository;
        _mediaRepository = mediaRepository;
        _mediaCustomerRepository = mediaCustomerRepository;
        _contractRepository = contractRepository;
        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
    }

    public async Task<PaginatedResult<CustomerListDto>> GetAll(CustomerInputDto input)
    {
        var query = (from customer in _customerRepository.Customers
                join customerCare in _customerCareRepository.CustomerCares
                    on customer.Id equals customerCare.CustomerId into c_cc
                from ccc in c_cc.DefaultIfEmpty()
                join employee in _employeeRepository.Employees
                    on ccc.EmployeeId equals employee.Id
                    into c_e
                from cc in c_e.DefaultIfEmpty()
                select new CustomerListDto()
                {
                    Id = customer.Id,
                    Address = customer.Address,
                    Avatar = "",
                    PhoneNumber = customer.PhoneNumber,
                    Education = customer.Education,
                    Job = customer.Job,
                    Marriage = customer.Marriage,
                    Status = customer.Status,
                    SalesEmployee = cc.FullName,
                    IdentityCard = customer.IdentityCard,
                    FullName = customer.FullName,
                    UserId = customer.UserId,
                    TotalMoney = customer.TotalMoney,
                    CreatedOn = customer.CreatedOn
                })
            ;
        var pageAndSorted = await query
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                x => x.FullName.Contains(input.Filter)
                     || x.PhoneNumber.Contains(input.Filter)
                     || x.IdentityCard.Contains(input.Filter)
            )
            .OrderBy(input.Sorting)
            .ToPaginatedListAsync(input.PageNumber, input.PageSize);

        foreach (var item in pageAndSorted.Data)
        {
            item.Avatar = await GetAvatar(item.Id);
        }

        return pageAndSorted;
    }

    private async Task<string> GetAvatar(long id)
    {
        var avatar = await _mediaCustomerService.GetAvatarCustomer(id);
        return avatar;
    }

    public async Task<CustomersForDashboardDto> GetCustomerForDashboard()
    {
        var result = new CustomersForDashboardDto();
        var customers = await _customerRepository.GetListAsync();
        var customersRegisterToday = customers.Where(x => x.CreatedOn.Date.Equals(DateTime.Now.Date));
        var customersAuthenticatedToday = customers.Where(x => x.CreatedOn.Date.Equals(DateTime.Now.Date) &&  x.Status == CustomerStatus.Verified.ToString());
        result.TotalCustomer = customers.Count();
        result.TotalCustomersRegisteredToday = customersRegisterToday.Count();
        result.TotalCustomersAuthenticatedToday = customersAuthenticatedToday.Count();
        result.TotalCustomersAuthenticated = customers.Count(x => x.Status == CustomerStatus.Verified.ToString());
        return result;
    }

    public async Task CreateOrUpdate(CreateOrEditCustomerDto input)
    {
        if (input.Id > 0)
        {
            await Update(input);
        }
        else
        {
            await Create(input);
        }
    }

    private async Task Create(CreateOrEditCustomerDto input)
    {
        var data = _mapper.Map<Customer>(input);
        await _customerRepository.InsertAsync(data);
        await _unitOfWork.Commit();
    }

    private async Task Update(CreateOrEditCustomerDto input)
    {
        var customer = await _customerRepository.GetByIdAsync(input.Id);
        var data = _mapper.Map(input, customer);
        await _customerRepository.UpdateAsync(data);
        await _customerCacheRepository.UpdateAsync(data);
        await _unitOfWork.Commit();
    }

    public async Task<CreateOrEditCustomerDto> GetById(long id)
    {
        var data = await _customerRepository.GetByIdAsync(id);
        var result = _mapper.Map<CreateOrEditCustomerDto>(data);
        return result;
    }

    public async Task<GetCustomerForDetail> GetForDetail(long id)
    {
        var data = await _customerRepository.Customers.Include(x => x.Bank).Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        var result = _mapper.Map<GetCustomerForDetail>(data);
        result.Avatar = await GetAvatar(id);
        return result;
    }

    public async Task Delete(long id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == customer.UserId);
        await _userManager.DeleteAsync(user);
        var data = await _customerRepository.GetByIdAsync(id);
        await _customerRepository.DeleteAsync(data);
        await _customerCacheRepository.DeleteByIdAsync(data.Id);
        await _unitOfWork.Commit();
    }

    public async Task<bool> CheckVerifiedByCurrentUser()
    {
        var customer = await GetCustomerByUser(_authenticatedUser.UserId);
        return await CheckVerified(customer.Id);
    }

    public async Task<bool> CheckVerified(long id)
    {
        var customer =
            await _customerRepository.Customers.AnyAsync(x =>
                x.Id == id && x.Status == CustomerStatus.Verified.ToString());
        return customer;
    }

    public async Task<bool> CheckIdentityExist(string number)
    {
        return await _customerRepository.Customers.AnyAsync(x => x.IdentityCard == number);
    }

    public async Task VerifiedCustomer(long customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        customer.Status = CustomerStatus.Verified.ToString();
        await _customerRepository.UpdateAsync(customer);
        await _unitOfWork.Commit();
    }

    public async Task<List<CustomerExcelDto>> GetCustomerExcel(CustomerExcelInputDto input)
    {
        var data = await _customerRepository.Customers.Include(x => x.Contract).ThenInclude(x => x.Interest)
            .Where(x => x.CreatedOn.Date >= input.StartDate && x.CreatedOn.Date <= input.EndDate.Date)
            .Select(x => new CustomerExcelDto()
            {
                Id = x.Id,
                Status = x.Contract.Status,
                CreatedOn = x.CreatedOn,
                FullName = x.FullName,
                InterestName = x.Contract.Interest.Name,
                PhoneNumber = x.PhoneNumber,
                AmountOfMoney = x.Contract.AmountOfMoney
            }).ToListAsync();
        return data;
    }

    public async Task<GetCustomerForDetail> GetCustomerByUser(string id)
    {
        var data = await _customerRepository.Customers.FirstOrDefaultAsync(x => x.UserId == id);
        var result = _mapper.Map<GetCustomerForDetail>(data);
        return result;
    }

    public async Task<ContractDto> CreateCustomerContract(MediaDto file, long customerId, long interestId, decimal money)
    {
        using (var transaction =
               await _applicationDbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted))
        {
            try
            {
                var media = _mapper.Map<Media>(file);
                var customer = await _customerRepository.GetByIdAsync(customerId);
                await _mediaRepository.InsertAsync(media);
                await _unitOfWork.Commit();
                var contract = new Contract()
                {
                    AmountOfMoney = money,
                    Status = ContractStatus.Pending.ToString(),
                    InterestId = interestId,
                    DigitalSignature = media.Id,
                    ContractCode = $"{DateTime.Now.ToString("d")}-{DateTime.Now.Ticks.ToString()}"
                };
                await _contractRepository.InsertAsync(contract);
                await _unitOfWork.Commit();

                customer.ContractId = contract.Id;
                await _customerRepository.UpdateAsync(customer);
                await _unitOfWork.Commit();
                await transaction.CommitAsync();
                return _mapper.Map<ContractDto>(contract);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
            }
        }

        return null;
    }
}