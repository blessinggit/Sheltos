using Microsoft.AspNetCore.Identity;
using Sheltos.Models;

namespace Sheltos.Data
{
    public static class SeedRoles
    {
        public static async Task EnsureRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Agent", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var adminEmail = "nwekeblessing06@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser != null && !(await userManager.IsInRoleAsync(adminUser, "Admin")))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
