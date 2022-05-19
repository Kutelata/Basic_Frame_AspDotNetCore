using Loan2022.Domain.Abstracts;

namespace Loan2022.Domain.Entities;

public class Setting: BaseEntity
{
    public string Key { get; set; }
    public string? Value { get; set; }
}