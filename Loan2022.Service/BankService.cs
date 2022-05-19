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
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Service;

public class BankService : IBankService
{
    private readonly IBankRepository _bankRepository;
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;
    private readonly IBankCacheRepository _bankCacheRepository;

    public BankService(
        IBankRepository bankRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IBankCacheRepository bankCacheRepository
    )
    {
        _bankRepository = bankRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _bankCacheRepository = bankCacheRepository;
    }

    public async Task<PaginatedResult<BankDto>> GetAll(BankInputDto input)
    {
        var query = _bankRepository.Banks
            .Select(x => new BankDto
            {
                Id = x.Id,
                Logo = x.Logo,
                BankName = x.BankName
            });
        var pageAndSorted = await query
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                x => x.BankName.Contains(input.Filter))
            .OrderBy(input.Sorting)
            .ToPaginatedListAsync(input.PageNumber, input.PageSize);
        return pageAndSorted;
    }

    public async Task<List<BankDto>> GetAllBank()
    {
        var query = await _bankRepository.Banks
            .Select(x => new BankDto
            {
                Id = x.Id,
                Logo = x.Logo,
                BankName = x.BankName
            }).ToListAsync();
        return query;
    }

    public async Task CreateOrUpdate(BankDto input)
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

    private async Task Create(BankDto input)
    {
        var data = _mapper.Map<Bank>(input);
        await _bankRepository.InsertAsync(data);
        await _unitOfWork.Commit();
    }

    private async Task Update(BankDto input)
    {
        var bank = await _bankRepository.GetByIdAsync(input.Id);
        _mapper.Map(input, bank);
        await _bankRepository.UpdateAsync(bank);
        await _bankCacheRepository.UpdateAsync(bank);
        await _unitOfWork.Commit();
    }

    public async Task<BankDto> GetById(long id)
    {
        var data = await _bankCacheRepository.GetByIdAsync(id);
        var result = _mapper.Map<BankDto>(data);
        return result;
    }

    public async Task Delete(long id)
    {
        var data = await _bankRepository.GetByIdAsync(id);
        await _bankRepository.DeleteAsync(data);
        await _bankCacheRepository.DeleteByIdAsync(data.Id);
        await _unitOfWork.Commit();
    }

    public async Task<List<BankDto>> GetAll()
    {
        var data = await _bankRepository.GetListAsync();
        return _mapper.Map<List<BankDto>>(data).OrderBy(x=>x.BankName).ToList();
    }
}