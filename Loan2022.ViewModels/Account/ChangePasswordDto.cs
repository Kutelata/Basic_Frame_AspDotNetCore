using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Account;

public class ChangePasswordDto
{
    public string UserId { get; set; }
    public string Password { get; set; }
    public string RePassword { get; set; }
}