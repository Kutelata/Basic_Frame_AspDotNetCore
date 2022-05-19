using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Contract;

public class ContractDto : BaseEntity
{
    public string Status { get; set; }
    public long? DigitalSignature { get; set; }
    public Decimal AmountOfMoney { get; set; }
    public string ContractCode { get; set; }
    public string? Reason { get; set; }
    public long? InterestId { get; set; }
    public string InterestName { get; set; }
    public bool IsWithdrawMoney { get; set; }
    public DateTime CreatedOn { get; set; }
    public Decimal Percent { get; set; }
}