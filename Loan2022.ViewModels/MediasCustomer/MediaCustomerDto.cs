using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.MediasCustomer;

public class MediaCustomerDto:BaseEntity
{
    public long CustomerId { get; set; }  
    public long MediaId { get; set; }  
    public string Type { get; set; } 
}