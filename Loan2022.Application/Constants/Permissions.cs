using Loan2022.Application.Enums;

namespace Loan2022.Application.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.{PermissionTypes.Create.ToString()}",
                $"Permissions.{module}.{PermissionTypes.View.ToString()}",
                $"Permissions.{module}.{PermissionTypes.Edit.ToString()}",
                $"Permissions.{module}.{PermissionTypes.Delete.ToString()}",
            };
        }
    }

    public static class CustomerPermissions
    {
        public const string Module = "Customers";
        public const string View = $"Permissions.{Module}.View";
        public const string Create = $"Permissions.{Module}.Create";
        public const string Edit = $"$Permissions.{Module}.Edit";
        public const string Delete = $"Permissions.{Module}.Delete";
    }
    public static class EmployeePermissions
    {
        public const string Module = "Employees";
        public const string View = $"Permissions.{Module}.View";
        public const string Create = $"Permissions.{Module}.Create";
        public const string Edit = $"$Permissions.{Module}.Edit";
        public const string Delete = $"Permissions.{Module}.Delete";
    }
}