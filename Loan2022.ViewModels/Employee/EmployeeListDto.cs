using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Employee;

public class EmployeeListDto:BaseEntity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Status { get; set; }
    public string ChatId { get; set; }
    public int TotalCare { get; set; }
    public int TotalToday { get; set; }
}