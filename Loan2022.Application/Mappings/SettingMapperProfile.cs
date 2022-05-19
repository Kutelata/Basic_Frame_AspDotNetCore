using AutoMapper;
using Loan2022.Domain.Entities;
using Loan2022.ViewModels.Setting;

namespace Loan2022.Application.Mappings;

internal class SettingMapperProfile: Profile
{
    public SettingMapperProfile()
    {
        CreateMap<SettingDto, Setting>().ReverseMap();
    }
}