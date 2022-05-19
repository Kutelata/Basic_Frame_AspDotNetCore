using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Loan2022.Domain.Abstracts;

namespace Loan2022.Domain.Entities;

[Table("Customers")]
public class Customer : AuditableEntity
{
    [MaxLength(50)] public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Job { get; set; }
    public string? IdentityCard { get; set; }
    [MaxLength(20)] public string? PhoneNumber { get; set; }
    [MaxLength(500)] public string? Address { get; set; }
    [MaxLength(10)] public int? Education { get; set; }
    [MaxLength(50)] public int? Marriage { get; set; }
    [MaxLength(50)] public string? AccountNumber { get; set; }
    public int? Income { get; set; }
    public long? BankId { get; set; }
    public virtual Bank Bank { get; set; }
    [MaxLength(50)] public string? BeneficiaryOfName { get; set; }
    public string Status { get; set; }
    public string UserId { get; set; }
    public int CurrentStepVerify  { get; set; }
    
    public Decimal TotalMoney { get; set; }
    
    public long? ContractId { get; set; }
    public virtual Contract Contract { get; set; }
}