using Loan2022.Application.Constants;
using Microsoft.AspNetCore.Identity;
using Loan2022.Infrastructure.Identity.Models;

namespace Loan2022.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            await roleManager.CreateAsync(new IdentityRole(Roles.Employee));
            await roleManager.CreateAsync(new IdentityRole(Roles.Customer));
        }
    }
}