using System.ComponentModel.DataAnnotations.Schema;
using Loan2022.Domain.Abstracts;

namespace Loan2022.Domain.Entities;
[Table("CustomersCare")]
public class CustomerCare:AuditableEntity
{
    [ForeignKey("Customer")]
    public long CustomerId { get; set; }  
    public virtual Customer Customer { get; set; }  
    
    [ForeignKey("Employee")]
    public long EmployeeId { get; set; }  
    public virtual Employee Employee { get; set; }
    
    public int Status {get; set; }
}