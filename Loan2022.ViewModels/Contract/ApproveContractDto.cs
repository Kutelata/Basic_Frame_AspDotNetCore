namespace Loan2022.ViewModels.Contract;

public class ApproveContractDto
{
    public long ContractId { get; set; }
    public long CustomerId { get; set; }
    public Decimal AmountOfMoney { get; set; }
}