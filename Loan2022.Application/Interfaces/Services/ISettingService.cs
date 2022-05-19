using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Setting;

namespace Loan2022.Application.Interfaces.Services;

public interface ISettingService
{
    Task<PaginatedResult<SettingDto>> GetAll(SettingInputDto input);
    Task CreateOrUpdate(SettingDto input);
    Task<SettingDto> GetById(long id);
    Task<SettingDto> GetByName(string key);
    Task Delete(long id);
}