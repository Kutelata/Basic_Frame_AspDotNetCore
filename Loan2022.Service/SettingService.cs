using AutoMapper;
using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Domain.Entities;
using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using System.Linq.Dynamic.Core;
using Loan2022.ViewModels.Setting;
using Microsoft.EntityFrameworkCore;
namespace Loan2022.Service;

public class SettingService : ISettingService
{
    private readonly ISettingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public SettingService(
        ISettingRepository repository,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<PaginatedResult<SettingDto>> GetAll(SettingInputDto input)
    {
        var query = _repository.Settings
            .Select(x => new SettingDto
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value
            });
        var pageAndSorted = await query
            .OrderBy(input.Sorting)
            .ToPaginatedListAsync(input.PageNumber, input.PageSize);
        return pageAndSorted;
    }
    
    public async Task CreateOrUpdate(SettingDto input)
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

    private async Task Create(SettingDto input)
    {
        var data = _mapper.Map<Setting>(input);
        await _repository.InsertAsync(data);
        await _unitOfWork.Commit();
    }

    private async Task Update(SettingDto input)
    {
        var data = await _repository.GetByIdAsync(input.Id);
        _mapper.Map(input, data);
        await _repository.UpdateAsync(data);
        await _unitOfWork.Commit();
    }

    public async Task<SettingDto> GetById(long id)
    {
        var data = await _repository.GetByIdAsync(id);
        var result = _mapper.Map<SettingDto>(data);
        return result;
    }

    public async Task<SettingDto> GetByName(string key)
    {
        var data = await _repository.Settings.Where(x=>x.Key == key).FirstOrDefaultAsync();
        var result = _mapper.Map<SettingDto>(data);
        return result;
    }

    public async Task Delete(long id)
    {
        var data = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(data);
        await _unitOfWork.Commit();
    }
}