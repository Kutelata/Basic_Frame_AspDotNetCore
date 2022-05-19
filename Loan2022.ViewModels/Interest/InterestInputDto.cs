namespace Loan2022.ViewModels.Interest;

public class InterestInputDto
{
    public string Filter { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Sorting { get; set; }
    public InterestInputDto()
    {
        if (String.IsNullOrEmpty(Sorting))
        {
            Sorting = "Id";
        }
    }
}