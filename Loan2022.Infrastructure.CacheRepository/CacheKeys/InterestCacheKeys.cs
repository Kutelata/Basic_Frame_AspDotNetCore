namespace Loan2022.Infrastructure.CacheRepository.CacheKeys;

public class InterestCacheKeys
{
    public static string GetKey(long interestId) => $"Interest-{interestId}";
    public static string ListKey => "InterestList";
    // public static string GetListKey(string name) => $"Banks-{name}";
}