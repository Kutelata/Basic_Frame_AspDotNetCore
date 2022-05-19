using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Loan2022.Domain.Abstracts;

namespace Loan2022.Domain.Entities;

[Table("Employees")]
public class Employee : AuditableEntity
{
    [MaxLength(50)] public string FullName { get; set; }
    [MaxLength(50)] public string PhoneNumber { get; set; }
    public string Status { get; set; }
    public string ChatId { get; set; }
    public string UserId { get; set; }
    public virtual List<CustomerCare> CustomerCares { get; set; }
}