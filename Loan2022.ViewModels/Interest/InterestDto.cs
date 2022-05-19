using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Interest;

public class InterestDto:BaseEntity
{
    public string Name { get; set; }
    public Decimal Percent { get; set; }
    public int NumberOfMonth { get; set; }
}