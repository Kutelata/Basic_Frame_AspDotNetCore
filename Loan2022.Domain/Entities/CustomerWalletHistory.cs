using System.ComponentModel.DataAnnotations.Schema;
using Loan2022.Domain.Abstracts;

namespace Loan2022.Domain.Entities;

[Table("CustomerWalletHistorys")]
public class CustomerWalletHistory : AuditableEntity
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description  { get; set; }
    public Decimal? Amount { get; set; }
    [ForeignKey("Customer")] public long CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
}