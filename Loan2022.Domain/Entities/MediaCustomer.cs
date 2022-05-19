using System.ComponentModel.DataAnnotations.Schema;
using Loan2022.Domain.Abstracts;

namespace Loan2022.Domain.Entities;
[Table("MediasCustomer")]
public class MediaCustomer:BaseEntity
{
    [ForeignKey("Customer")]
    public long CustomerId { get; set; }  
    public virtual Customer Customer { get; set; }

    [ForeignKey("Media")]
    public long MediaId { get; set; }  
    public virtual Media Media { get; set; }

    public string Type { get; set; }  
}