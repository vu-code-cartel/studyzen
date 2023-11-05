using Microsoft.AspNetCore.Identity;

namespace StudyZen.Infrastructure;

public static class SeedData
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager, string[] roles)
    {
        foreach (var role in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}