using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Bank;

public class CustomerWalletHistoryDto : BaseEntity
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public Decimal Amount { get; set; }
    public long CustomerId { get; set; }
}