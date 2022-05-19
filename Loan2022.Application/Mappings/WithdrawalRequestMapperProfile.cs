using AutoMapper;
using Loan2022.Domain.Entities;
using Loan2022.ViewModels.WithdrawalRequest;

namespace Loan2022.Application.Mappings;

internal class WithdrawalRequestMapperProfile: Profile
{
    public WithdrawalRequestMapperProfile()
    {
        CreateMap<WithdrawalRequestDto, WithdrawalRequest>().ReverseMap();
    }
}