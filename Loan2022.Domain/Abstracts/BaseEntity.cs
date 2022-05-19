using Loan2022.Domain.Interfaces;

namespace Loan2022.Domain.Abstracts;

public abstract class BaseEntity: IBaseEntity
{
    public long Id { get; set; }
}