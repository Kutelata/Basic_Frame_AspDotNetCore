using AutoMapper;
using Loan2022.Application.Enums;
using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Domain.Entities;
using Loan2022.Result;
using Loan2022.Service.Extensions;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Contract;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Service;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerService _customerService;
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;
    private readonly IContractCacheRepository _contractCacheRepository;
    private readonly ICustomerCacheRepository _customerCacheRepository;

    public ContractService(
        IContractRepository contractRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IContractCacheRepository contractCacheRepository,
        ICustomerService customerService,
        ICustomerRepository customerRepository,
        ICustomerCacheRepository customerCacheRepository
    )
    {
        _contractRepository = contractRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _contractCacheRepository = contractCacheRepository;
        _customerRepository = customerRepository;
        _customerCacheRepository = customerCacheRepository;
    }

    public async Task<PaginatedResult<ContractDto>> GetAll(ContractInputDto input)
    {
        var lst = await _contractRepository.GetListAsync();
        var query = await _contractRepository.Contracts
            .Select(x => _mapper.Map<ContractDto>(x)).ToPaginatedListAsync(input.PageNumber, input.PageSize);
        query.Data = _mapper.Map<List<ContractDto>>(query.Data);
        return query;
    }

    public async Task<ContractDto> GetContractCustomer(long customerId)
    {
        var output = new ContractDto();
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer.ContractId <= 0)
        {
            return output;
        }

        var contract = await _contractRepository.Contracts.Include(x => x.Interest)
            .Where(x => x.Id == customer.ContractId).FirstOrDefaultAsync();
        var result = _mapper.Map<ContractDto>(contract);
        return result;
    }

    public async Task ApproveContract(ApproveContractDto input)
    {
        var contract =await _contractRepository.GetByIdAsync(input.ContractId);
        contract.Status=  ContractStatus.Approved.ToString();
        await _contractRepository.UpdateAsync(contract);
        await _unitOfWork.Commit();
    }

    public async Task CreateOrUpdate(ContractDto input)
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

    private async Task Create(ContractDto input)
    {
        var data = _mapper.Map<Contract>(input);
        await _contractRepository.InsertAsync(data);
        await _unitOfWork.Commit();
    }

    private async Task Update(ContractDto input)
    {
        var contract = await _contractRepository.GetByIdAsync(input.Id);
        _mapper.Map(input, contract);
        await _contractRepository.UpdateAsync(contract);
        await _contractCacheRepository.UpdateAsync(contract);
        await _unitOfWork.Commit();
    }
    

    public async Task<ContractDto> GetById(long id)
    {
        var data = await _contractRepository.GetByIdAsync(id);
        var result = _mapper.Map<ContractDto>(data);
        return result;
    }

    public async Task Delete(long id)
    {
        var data = await _contractRepository.GetByIdAsync(id);
        await _contractRepository.DeleteAsync(data);
        await _contractCacheRepository.DeleteByIdAsync(data.Id);
        await _unitOfWork.Commit();
    }

    public async Task<bool> CheckAnyContractNotApprovedByCustomer(long id)
    {
        // var contracts =
        //     _contractRepository.Contracts.AnyAsync(x =>
        //         x.CustomerId == id && x.Status != ContractStatus.Finished.ToString());
        // return contracts;
        var isValid = await _customerRepository.Customers.Include(x => x.Contract)
            .Where(x => x.Id == id && x.Contract==null)
            .AnyAsync();
        return isValid;
    }
}