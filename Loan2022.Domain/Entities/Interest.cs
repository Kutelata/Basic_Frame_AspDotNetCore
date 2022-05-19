using System.ComponentModel.DataAnnotations.Schema;
using Loan2022.Domain.Abstracts;

namespace Loan2022.Domain.Entities;

[Table("Interests")]
public class Interest:BaseEntity
{
    public string Name { get; set; }
    public Decimal Percent { get; set; }
    public int NumberOfMonth { get; set; }
}