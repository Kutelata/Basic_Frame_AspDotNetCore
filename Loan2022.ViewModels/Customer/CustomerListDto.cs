using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Customer;

public class CustomerListDto:BaseEntity
{
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Job { get; set; }
    public string IdentityCard { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public int? Education { get; set; }
    public int? Marriage { get; set; }
    public int? Income { get; set; }
    public string AccountNumber { get; set; }
    public long BankId { get; set; }
    public string BankName { get; set; }
    public string BeneficiaryOfName { get; set; }
    public Decimal? MoneyBalance { get; set; }
    public string SalesEmployee { get; set; }
    public string Avatar { get; set; }
    public string Status { get; set; }
    public string UserId { get; set; }
    public Decimal TotalMoney { get; set; }
    public DateTime CreatedOn { get; set; } 
}