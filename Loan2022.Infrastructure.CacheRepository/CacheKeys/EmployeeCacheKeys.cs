namespace Loan2022.Infrastructure.CacheRepository.CacheKeys;

public class EmployeeCacheKeys
{
    public static string GetKey(long employeeId) => $"Employee-{employeeId}";
    public static string ListKey => "EmployeeList";
}