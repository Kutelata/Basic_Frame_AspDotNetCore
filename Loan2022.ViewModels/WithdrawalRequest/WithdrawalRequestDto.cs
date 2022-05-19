using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.WithdrawalRequest;
public class WithdrawalRequestDto : BaseEntity
{
    public long CustomerId { get; set; }
    public Decimal AmountOfMoney { get; set; }
    public string? Status { get; set; }
    public string Name { get; set; }
    public DateTime CreatedOn { get; set; }
}