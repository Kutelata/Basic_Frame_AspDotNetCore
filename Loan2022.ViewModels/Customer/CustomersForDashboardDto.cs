using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Customer;

public class CustomersForDashboardDto
{
    public int TotalCustomer { get; set; }
    public int TotalCustomersRegisteredToday { get; set; }
    public int TotalCustomersAuthenticatedToday { get; set; }
    public int TotalCustomersAuthenticated { get; set; }
}