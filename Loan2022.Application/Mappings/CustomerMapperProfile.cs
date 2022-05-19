using AutoMapper;
using Loan2022.Domain.Entities;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Customer;

namespace Loan2022.Application.Mappings;

internal class CustomerMapperProfile : Profile
{
    public CustomerMapperProfile()
    {
        CreateMap<CreateOrEditCustomerDto, Customer>().ReverseMap();
        CreateMap<Customer, CustomerListDto>();
        CreateMap<Customer, GetCustomerForDetail>()
            .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.Bank.BankName));
        CreateMap<CreateOrEditCustomerDto, GetCustomerForDetail>().ReverseMap();
        CreateMap<CreateOrEditCustomerDto, CustomerInformationInput>().ReverseMap();
        CreateMap<CreateOrEditCustomerDto, CustomerBankInformationInput>().ReverseMap();
    }
}