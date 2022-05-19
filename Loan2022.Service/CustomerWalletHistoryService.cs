using AutoMapper;
using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Domain.Entities;
using Loan2022.Result;
using Loan2022.Service.Extensions;
using Loan2022.ViewModels.Bank;
using System.Linq.Dynamic.Core;

namespace Loan2022.Service;

public class CustomerWalletHistoryService : ICustomerWalletHistoryService
{
    private readonly ICustomerWalletHistoryRepository _repository;
    private readonly ICustomerService _customerService;
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public CustomerWalletHistoryService(
        ICustomerWalletHistoryRepository repository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ICustomerService customerService
    )
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _customerService = customerService;
    }

    public async Task<List<CustomerWalletHistoryDto>> GetCustomerWalletHistoryByCustomerId(long id)
    {
        var query = _repository.CustomerWalletHistorys.Where(x => x.CustomerId == id);
        var result = _mapper.Map<List<CustomerWalletHistoryDto>>(query);
        return await Task.FromResult(result);
    }

    public async Task<bool> Create(CustomerWalletHistoryDto input)
    {
        var data = _mapper.Map<CustomerWalletHistory>(input);
        var isCreate = await UpdateTotalMoneyCustomer(input);
        if (!isCreate)
        {
            return false;
        }
        else
        {
            await _repository.InsertAsync(data);
            await _unitOfWork.Commit();
            return true;
        }
    }

    public async Task<bool> UpdateTotalMoneyCustomer(CustomerWalletHistoryDto input)
    {
        var customer = await _customerService.GetById(input.CustomerId);
        if (input.Type == "Plus")
        {
            customer.TotalMoney += input.Amount;
        }
        else
        {
            if (input.Amount > customer.TotalMoney)
            {
                return false;
            }

            customer.TotalMoney -= input.Amount;
        }

        await _customerService.CreateOrUpdate(customer);
        return true;
    }

    public async Task Update(CustomerWalletHistoryDto input)
    {
        var data = await _repository.GetByIdAsync(input.Id);
        _mapper.Map(input, data);
        await _repository.UpdateAsync(data);
        await _unitOfWork.Commit();
    }

    public async Task<CustomerWalletHistoryDto> GetById(long id)
    {
        var data = await _repository.GetByIdAsync(id);
        var result = _mapper.Map<CustomerWalletHistoryDto>(data);
        return result;
    }

    public async Task Delete(long id)
    {
        var data = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(data);
        await _unitOfWork.Commit();
    }
}