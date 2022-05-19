namespace Loan2022.ViewModels.Customer;

public class CustomerInputDto
{
  
    public string Filter { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Sorting { get; set; }
    public CustomerInputDto()
    {
        if (String.IsNullOrEmpty(Sorting))
        {
            Sorting = "CreatedOn DESC";
        }
    }
}