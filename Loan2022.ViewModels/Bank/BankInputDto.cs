namespace Loan2022.ViewModels.Bank;

public class BankInputDto
{
    public string Filter { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Sorting { get; set; }
    public BankInputDto()
    {
        if (String.IsNullOrEmpty(Sorting))
        {
            Sorting = "Id";
        }
    }
}