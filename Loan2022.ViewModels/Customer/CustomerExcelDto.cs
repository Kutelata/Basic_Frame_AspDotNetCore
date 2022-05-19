using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Customer;

public class CustomerExcelDto:BaseEntity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public Decimal? AmountOfMoney { get; set; }
    public string InterestName { get; set; }
    public string Status { get; set; }
    public DateTime CreatedOn { get; set; }
}