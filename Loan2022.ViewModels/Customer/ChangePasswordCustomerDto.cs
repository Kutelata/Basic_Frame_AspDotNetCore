using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Customer;

public class ChangePasswordCustomerDto
{
    public string userId { get; set; }
    public string Password { get; set; }
    public string RePassword { get; set; }
}