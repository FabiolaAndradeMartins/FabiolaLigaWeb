using LigaWeb.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace LigaWeb.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            
            var adminUser = await userManager.FindByEmailAsync("bidelavitta1@gmail.com");
            if (adminUser == null)
            {
                
                var user = new User
                {
                    UserName = "bidelavitta1@gmail.com",
                    Email = "bidelavitta1@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Fabiola",
                    LastName = "Martins",                    
                    TwoFactorEnabled = false,
                };
                
                var result = await userManager.CreateAsync(user, "123456");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
