using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Loan2022.Application.Constants;
using Loan2022.Application.Enums;
using Loan2022.Infrastructure.Identity.Models;

namespace Loan2022.Infrastructure.Identity.Seeds
{
    public static class DefaultSuperAdminUser
    {
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
                }
            }
        }

        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
            await roleManager.AddPermissionClaim(adminRole, CustomerPermissions.Module);
            await roleManager.AddPermissionClaim(adminRole, EmployeePermissions.Module);
        }

        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "It@123456");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Employee);
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin);
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin);
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
            
            var defaultUserAdmin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            
            if (userManager.Users.All(u => u.Id != defaultUserAdmin.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUserAdmin.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUserAdmin, "It@123456");
                    await userManager.AddToRoleAsync(defaultUserAdmin, Roles.Employee);
                    await userManager.AddToRoleAsync(defaultUserAdmin, Roles.Admin);
                    await userManager.AddToRoleAsync(defaultUserAdmin, Roles.SuperAdmin);
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
            
        }
    }
}