using System.ComponentModel.DataAnnotations.Schema;
using Loan2022.Domain.Abstracts;

namespace Loan2022.Domain.Entities;
[Table("Medias")]
public class Media : AuditableEntity
{
    public string Path { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
}