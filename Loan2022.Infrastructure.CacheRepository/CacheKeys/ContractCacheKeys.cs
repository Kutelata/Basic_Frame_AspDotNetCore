namespace Loan2022.Infrastructure.CacheRepository.CacheKeys;

public class ContractCacheKeys
{
    public static string GetKey(long contractId) => $"Contract-{contractId}";
    public static string ListKey => "ContractList";
}