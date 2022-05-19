namespace Loan2022.Infrastructure.CacheRepository.CacheKeys;

public class BankCacheKeys
{
    public static string GetKey(long bankId) => $"Bank-{bankId}";
    public static string ListKey => "BankList";
    // public static string GetListKey(string name) => $"Banks-{name}";
}