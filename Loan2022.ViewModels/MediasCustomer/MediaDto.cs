using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.MediasCustomer;

public class  MediaDto: BaseEntity
{
    public string Path { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public string Type { get; set; }
}