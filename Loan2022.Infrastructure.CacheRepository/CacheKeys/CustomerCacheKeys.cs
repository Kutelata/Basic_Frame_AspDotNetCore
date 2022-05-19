namespace Loan2022.Infrastructure.CacheRepository.CacheKeys;

public class CustomerCacheKeys
{
    public static string GetKey(long customerId) => $"Customer-{customerId}";
    public static string ListKey => "CustomerList";
}