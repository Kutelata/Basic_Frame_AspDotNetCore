using AutoMapper;
using Loan2022.Application.Constants;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Domain.Entities;
using Loan2022.Infrastructure.DbContexts;
using Loan2022.Infrastructure.Identity.Models;
using Loan2022.ViewModels.Account;
using Loan2022.ViewModels.Customer;
using Loan2022.ViewModels.Employee;
using Microsoft.AspNetCore.Identity;

namespace Loan2022.Service;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ICustomerService _customerService;
    private readonly IEmployeeService _employeeService;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerCareRepository _customerCareRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(UserManager<ApplicationUser> userManager,
        ApplicationDbContext applicationDbContext,
        ICustomerService customerService,
        IEmployeeService employeeService,
        ICustomerCareRepository customerCareRepository,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _userManager = userManager;
        _applicationDbContext = applicationDbContext;
        _customerService = customerService;
        _employeeService = employeeService;
        _customerCareRepository = customerCareRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }

    public async Task<bool> CreateUserCustomer(RegisterInput input, CreateOrEditCustomerDto customerDto)
    {
        var user = new ApplicationUser
        {
            UserName = input.Username
        };
        await using var dbContextTransaction = await _applicationDbContext.Database.BeginTransactionAsync();
        try
        {
            var userResult = await _userManager.CreateAsync(user, input.Password);
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Customer);
            if (userResult.Succeeded && roleResult.Succeeded)
            {
                customerDto.UserId = user.Id;
                var customer = _mapper.Map<Customer>(customerDto);
                await _customerRepository.InsertAsync(customer);
                await _unitOfWork.Commit();
                if (customerDto.EmployeeId.HasValue)
                {
                    await _customerCareRepository.InsertAsync(new CustomerCare()
                    {
                        CustomerId = customer.Id,
                        EmployeeId = customerDto.EmployeeId.Value
                    });
                   await _unitOfWork.Commit();
                }
                await dbContextTransaction.CommitAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            await dbContextTransaction.RollbackAsync();
        }

        return false;
    }

    public async Task<bool> CreateUserEmployee(RegisterInput input, CreateOrEditEmployeeDto employeeDto)
    {
        var user = new ApplicationUser
        {
            UserName = input.Username
        };
        await using var dbContextTransaction = await _applicationDbContext.Database.BeginTransactionAsync();
        try
        {
            var userResult = await _userManager.CreateAsync(user, input.Password);
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Employee);
            if (userResult.Succeeded && roleResult.Succeeded)
            {
                employeeDto.UserId = user.Id;
                await _employeeService.CreateOrUpdate(employeeDto);
                await dbContextTransaction.CommitAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            await dbContextTransaction.RollbackAsync();
        }

        return false;
    }

    public async Task<bool> ChangePassword(ChangePasswordDto input)
    {
        await using var dbContextTransaction = await _applicationDbContext.Database.BeginTransactionAsync();
        try
        {
            var user = await _userManager.FindByIdAsync(input.UserId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPassword = await _userManager.ResetPasswordAsync(user, token, input.Password);
            var result = resetPassword.Succeeded;
            await dbContextTransaction.CommitAsync();
            return result;
        }
        catch (Exception e)
        {
            await dbContextTransaction.RollbackAsync();
        }
        return false;
    }
}