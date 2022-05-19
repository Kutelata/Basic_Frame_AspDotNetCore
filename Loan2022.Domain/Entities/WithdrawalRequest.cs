using System.ComponentModel.DataAnnotations.Schema;
using Loan2022.Domain.Abstracts;

namespace Loan2022.Domain.Entities;

[Table("WithdrawalRequests")]
public class WithdrawalRequest : AuditableEntity
{
    [ForeignKey("Customer")] public long CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public Decimal AmountOfMoney { get; set; }
    public string? Status { get; set; }
    public string Name { get; set; }
}