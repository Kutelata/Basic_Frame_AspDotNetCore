using AutoMapper;
using Loan2022.Domain.Entities;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Contract;

namespace Loan2022.Application.Mappings;

internal class ContractMapperProfile: Profile
{
    public ContractMapperProfile()
    {

        CreateMap<ContractDto, Contract>();
        CreateMap<Contract, ContractDto>()
            .ForMember(dest => dest.InterestName, opt => opt.MapFrom(src => src.Interest.Name))
            .ForMember(dest => dest.Percent, opt => opt.MapFrom(src => src.Interest.Percent));
    }
}