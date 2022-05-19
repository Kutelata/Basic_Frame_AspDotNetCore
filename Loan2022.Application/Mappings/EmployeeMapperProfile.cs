using AutoMapper;
using Loan2022.Domain.Entities;
using Loan2022.ViewModels.Employee;

namespace Loan2022.Application.Mappings;

public class EmployeeMapperProfile:Profile
{
   public EmployeeMapperProfile()
   {
      CreateMap<CreateOrEditEmployeeDto, Employee>().ReverseMap();
      CreateMap<Employee, EmployeeListDto>();
   }
}