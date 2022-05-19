using System.ComponentModel.DataAnnotations.Schema;
using Loan2022.Domain.Abstracts;

namespace Loan2022.Domain.Entities;

[Table("Contracts")]
public class Contract : AuditableEntity
{
    // [ForeignKey("Customer")] public long CustomerId { get; set; }
    // public virtual Customer Customer { get; set; }
    public Decimal AmountOfMoney { get; set; }
    public string Status { get; set; }
    public long? DigitalSignature { get; set; }
    public string ContractCode { get; set; }
    public string? Reason { get; set; }
    public long? InterestId { get; set; }
    public virtual Interest Interest { get; set; }
    public bool IsWithdrawMoney { get; set; }
}