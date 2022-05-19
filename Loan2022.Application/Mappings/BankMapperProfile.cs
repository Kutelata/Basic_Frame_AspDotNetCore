using AutoMapper;
using Loan2022.Domain.Entities;
using Loan2022.ViewModels.Bank;

namespace Loan2022.Application.Mappings;

internal class BankMapperProfile: Profile
{
    public BankMapperProfile()
    {
        CreateMap<BankDto, Bank>().ReverseMap();
    }
}