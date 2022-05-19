using Loan2022.Domain.Interfaces;

namespace Loan2022.Domain.Abstracts;

public abstract class AuditableEntity : IAuditableBaseEntity
{
    public long Id { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }
}