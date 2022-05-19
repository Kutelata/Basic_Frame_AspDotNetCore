using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Loan2022.Application.Constants;
using Loan2022.Application.Enums;
using Loan2022.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Infrastructure.Identity.Seeds
{
    public static class DefaultCustomerUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "0976793180",
                Email = "customer@customer.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == defaultUser.UserName);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "It@123456");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Customer);
                }
            }
        }
    }
}