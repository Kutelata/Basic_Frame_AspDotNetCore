namespace Loan2022.ViewModels.Customer;

public class CustomerInformationInput
{
    public string Job { get; set; }
    public string IdentityCard { get; set; }
    public string Address { get; set; }
    public int? Education { get; set; }
    public string FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? Income { get; set; }
    public int? Marriage { get; set; }
}

public class CustomerBankInformationInput
{
    public long BankId { get; set; }
    public string BeneficiaryOfName { get; set; }
    public string AccountNumber { get; set; }
}