using System.Linq;
using System.Linq.Dynamic.Core;
using AutoMapper;
using Loan2022.Application.Constants;
using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Domain.Entities;
using Loan2022.Infrastructure.DbContexts;
using Loan2022.Infrastructure.Identity.Models;
using Loan2022.Result;
using Loan2022.Service.Extensions;
using Loan2022.ViewModels.Employee;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thinktecture;

namespace Loan2022.Service;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IContractRepository _contractRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerCareRepository _customerCareRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmployeeCacheRepository _employeeCacheRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _applicationDbContext;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IEmployeeCacheRepository employeeCacheRepository,
        IContractRepository contractRepository,
        ICustomerRepository customerRepository,
        ICustomerCareRepository customerCareRepository,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext applicationDbContext
    )
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _employeeCacheRepository = employeeCacheRepository;
        _unitOfWork = unitOfWork;
        _contractRepository = contractRepository;
        _customerRepository = customerRepository;
        _customerCareRepository = customerCareRepository;
        _userManager = userManager;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<PaginatedResult<EmployeeListDto>> GetAll(EmployeeInputDto input)
    {
        var query = _employeeRepository.Employees
            .Include(x => x.CustomerCares)
            .Select(x => new EmployeeListDto
            {
                Id = x.Id,
                Status = x.Status,
                ChatId = x.ChatId,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                TotalCare = x.CustomerCares.Count(),
                TotalToday = x.CustomerCares.Where(t => t.CreatedOn.Date == DateTime.Now.Date).Count()
            });
        var pageAndSorted = await query
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                x => x.FullName.Contains(input.Filter)
                     || x.PhoneNumber.Contains(input.Filter)
                     || x.TotalCare.ToString().Contains(input.Filter)
                     || x.TotalToday.ToString().Contains(input.Filter)
            )
            .OrderBy(input.Sorting)
            .ToPaginatedListAsync(input.PageNumber, input.PageSize);
        return pageAndSorted;
    }

    public async Task CreateOrUpdate(CreateOrEditEmployeeDto input)
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

    private async Task Create(CreateOrEditEmployeeDto input)
    {
        var data = _mapper.Map<Employee>(input);
        await _employeeRepository.InsertAsync(data);
        await _unitOfWork.Commit();
    }

    private async Task Update(CreateOrEditEmployeeDto input)
    {
        var employee = await _employeeRepository.GetByIdAsync(input.Id);
        input.CreatedBy = employee.CreatedBy;
        input.CreatedOn = employee.CreatedOn;
        _mapper.Map(input, employee);
        await _employeeRepository.UpdateAsync(employee);
        await _employeeCacheRepository.UpdateAsync(employee);
        await _unitOfWork.Commit();
    }

    public async Task<CreateOrEditEmployeeDto> GetById(long id)
    {
        var data = await _employeeCacheRepository.GetByIdAsync(id);
        var result = _mapper.Map<CreateOrEditEmployeeDto>(data);
        return result;
    }

    public async Task Delete(long id)
    {
        var data = await _employeeRepository.GetByIdAsync(id);
        await _employeeRepository.DeleteAsync(data);
        await _employeeCacheRepository.DeleteByIdAsync(data.Id);
        await _unitOfWork.Commit();
    }

    public async Task UpdateStatus(long id, string status)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        employee.Status = status;
        await _employeeRepository.UpdateAsync(employee);
        // var statusUser = status == "Active";
        // var user = await _userManager.FindByIdAsync(employee.UserId);
        // await _userManager.SetLockoutEnabledAsync(user, statusUser);
        await _unitOfWork.Commit();
    }

    public async Task<long> GetRandom()
    {
        var lastCustomer =
            await _customerCareRepository.CustomerCares.OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();
        long i = 0;
        var query = _employeeRepository.Employees;
        if (lastCustomer != null)
        {
            var result = await _employeeRepository.Employees.Select(o => new
            {
                o.Id,
                RowNumber = EF.Functions.RowNumber(EF.Functions.OrderBy(o.Id))
            }).AsSubQuery().Where(x => x.Id == lastCustomer.EmployeeId).FirstOrDefaultAsync();
            if (result != null)
            {
                i = result.RowNumber;
            }
        }

        var em = await query.Skip((int) i).FirstOrDefaultAsync(x => x.Status == "Active") ??
                 await query.FirstOrDefaultAsync(x => x.Status == "Active");
        return em.Id;
    }

    public async Task<long> GetRamDomEmployee()
    {
        var employeesActive = _employeeRepository.Employees.Include(x=>x.CustomerCares).ThenInclude(x=>x.Customer)
            .Where(x => x.Status == "Active")
            .OrderBy((x=>x.Id))
            .Select( x=> new
            {
                employeeId = x.Id,
                totalCareToday = x.CustomerCares.Where(c=>c.CreatedOn.Date.Equals(DateTime.Now.Date)).Count()
            })
            .OrderBy(x=>x.totalCareToday)
            ;
        var result =await employeesActive.FirstOrDefaultAsync();
        return result.employeeId;
    }

    public async Task<CreateOrEditEmployeeDto> GetEmployeeByCustomer(long customerId)
    {
        var em = await _customerCareRepository.CustomerCares.Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
        return _mapper.Map<CreateOrEditEmployeeDto>(em?.Employee);
    }
}