using Loan2022.Domain.Abstracts;

namespace Loan2022.ViewModels.Setting;

public class SettingDto:BaseEntity
{
    public string Key { get; set; }
    public string? Value { get; set; }
}