namespace Loan2022.ViewModels.Employee;

public class EmployeeInputDto
{
    public string Filter { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Sorting { get; set; }
    public EmployeeInputDto()
    {
        if (String.IsNullOrEmpty(Sorting))
        {
            Sorting = "Id";
        }
    }
}