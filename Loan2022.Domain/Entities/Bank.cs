using System.ComponentModel.DataAnnotations.Schema;
using Loan2022.Domain.Abstracts;
namespace Loan2022.Domain.Entities;
[Table("Banks")]
public class Bank:BaseEntity
{
    public string BankName { get; set; }
    public string Logo { get; set; }
}