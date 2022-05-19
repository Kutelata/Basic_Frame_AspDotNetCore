using AutoMapper;
using Loan2022.Domain.Entities;
using Loan2022.ViewModels.Interest;

namespace Loan2022.Application.Mappings;

internal class InterestMapperProfile: Profile
{
    public InterestMapperProfile()
    {
        CreateMap<InterestDto, Interest>().ReverseMap();
    }
}