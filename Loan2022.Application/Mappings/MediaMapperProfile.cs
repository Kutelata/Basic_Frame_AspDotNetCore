using AutoMapper;
using Loan2022.Domain.Entities;
using Loan2022.ViewModels.MediasCustomer;

namespace Loan2022.Application.Mappings;

internal class MediaMapperProfile: Profile
{
    public MediaMapperProfile()
    {
        CreateMap<MediaDto, Media>().ReverseMap();
    }
}