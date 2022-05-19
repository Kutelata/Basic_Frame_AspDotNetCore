using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.Customer;
using Loan2022.ViewModels.WithdrawalRequest;

namespace Loan2022.Client.Models;

public class WithdrawInvoiceViewModel
{
    public BankDto Bank { get; set; }
    public GetCustomerForDetail Customer { get; set; }
    public WithdrawalRequestDto Request { get; set; }
    public DateTime CreatedAt { get; set; }
}