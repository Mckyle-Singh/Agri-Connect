using Agri_Connect.Models;
using Microsoft.AspNetCore.Identity;


namespace Agri_Connect.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Employee", "Farmer", "User" };

            // Create roles if they don't exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
