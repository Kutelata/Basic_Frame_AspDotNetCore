using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Employee;

public class CreateOrEditEmployeeDto : AuditableEntity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Status { get; set; }
    public string ChatId { get; set; }
    public string Email { get; set; }
    public string UserId { get; set; }
}