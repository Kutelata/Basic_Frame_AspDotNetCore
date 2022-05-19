using AutoMapper;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Domain.Entities;
using Loan2022.ViewModels.WithdrawalRequest;
using Microsoft.EntityFrameworkCore;
namespace Loan2022.Service;
public class WithdrawalRequestService : IWithdrawalRequestService
{
    private readonly IWithdrawalRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public WithdrawalRequestService(
        IWithdrawalRequestRepository repository,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<List<WithdrawalRequestDto>> GetAll(long customerId)
    {
        var query =await _repository.WithdrawalRequests.Where(x => x.CustomerId == customerId)
            .OrderByDescending(x=>x.CreatedOn).ToListAsync();
        var result = _mapper.Map<List<WithdrawalRequestDto>>(query);
        return result;
    }
    
    public async Task CreateOrUpdate(WithdrawalRequestDto input)
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

    public async Task<long> Create(WithdrawalRequestDto input)
    {
        var data = _mapper.Map<WithdrawalRequest>(input);
        await _repository.InsertAsync(data);
        await _unitOfWork.Commit();
        return data.Id;
    }

    private async Task Update(WithdrawalRequestDto input)
    {
        var data = await _repository.GetByIdAsync(input.Id);
        _mapper.Map(input, data);
        await _repository.UpdateAsync(data);
        await _unitOfWork.Commit();
    }

    public async Task<WithdrawalRequestDto> GetById(long id)
    {
        var data = await _repository.GetByIdAsync(id);
        var result = _mapper.Map<WithdrawalRequestDto>(data);
        return result;
    }

    public async Task Delete(long id)
    {
        var data = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(data);
        await _unitOfWork.Commit();
    }
}