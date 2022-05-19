using AutoMapper;
using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Domain.Entities;
using Loan2022.Result;
using Loan2022.Service.Extensions;
using System.Linq.Dynamic.Core;
using Loan2022.ViewModels.Interest;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Service;

public class InterestService : IInterestService
{
    private readonly IInterestRepository _interestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;
    private readonly IInterestCacheRepository _interestCacheRepository;

    public InterestService(
        IInterestRepository interestRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IInterestCacheRepository interestCacheRepository
    )
    {
        _interestRepository = interestRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _interestCacheRepository = interestCacheRepository;
    }

    public async Task<PaginatedResult<InterestDto>> GetAll(InterestInputDto input)
    {
        var query = _interestRepository.Interests
            .Select(x => new InterestDto
            {
                Id = x.Id,
                Name = x.Name,
                Percent = x.Percent,
                 NumberOfMonth= x.NumberOfMonth,
            });
        var pageAndSorted = await query
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                x => x.Name.Contains(input.Filter))
            .OrderBy(input.Sorting)
            .ToPaginatedListAsync(input.PageNumber, input.PageSize);
        return pageAndSorted;
    }   
    
    public async Task<List<InterestDto>> GetAllInterest()
    {
        var query =await _interestRepository.Interests
            .Select(x => new InterestDto
            {
                Id = x.Id,
                Name = x.Name,
                Percent = x.Percent,
                NumberOfMonth= x.NumberOfMonth
            }).ToListAsync();
        return query;
    }

    public async Task CreateOrUpdate(InterestDto input)
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

    private async Task Create(InterestDto input)
    {
        var data = _mapper.Map<Interest>(input);
        await _interestRepository.InsertAsync(data);
        await _unitOfWork.Commit();
    }

    private async Task Update(InterestDto input)
    {
        var interest = await _interestRepository.GetByIdAsync(input.Id);
        _mapper.Map(input, interest);
        await _interestRepository.UpdateAsync(interest);
        await _interestCacheRepository.UpdateAsync(interest);
        await _unitOfWork.Commit();
    }

    public async Task<InterestDto> GetById(long id)
    {
        var data = await _interestCacheRepository.GetByIdAsync(id);
        var result = _mapper.Map<InterestDto>(data);
        return result;
    }

    public async Task Delete(long id)
    {
        var data = await _interestRepository.GetByIdAsync(id);
        await _interestRepository.DeleteAsync(data);
        await _interestCacheRepository.DeleteByIdAsync(data.Id);
        await _unitOfWork.Commit();
    }
}