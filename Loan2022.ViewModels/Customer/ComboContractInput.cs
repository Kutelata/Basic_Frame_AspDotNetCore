namespace Loan2022.ViewModels.Customer;

public class ComboContractInput
{
    public decimal? Money { get; set; }
    public long? Month { get; set; }

    public bool IsNull()
    {
        return !Money.HasValue || !Month.HasValue;
    }
}