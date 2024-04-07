using System;
using System.Threading.Tasks;
using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using Microsoft.AspNetCore.Identity;

public class DataSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Enum.GetValues(typeof(UserRoles)))
        {
            if (!await roleManager.RoleExistsAsync(role.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = role.ToString(),

                });
            }
        }
    }

    public static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        var userDb = await userManager.FindByNameAsync("superadmin");
        if (userDb == null)
        {
            var user = new AppUser
            {
                Email = "eljanash@code.edu.az",
                UserName = "superadmin",
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, "Superadmin123@");
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    throw new Exception(error.Description);
                }
            }
            await userManager.AddToRoleAsync(user, UserRoles.SuperAdmin.ToString());
        }
    }
}
