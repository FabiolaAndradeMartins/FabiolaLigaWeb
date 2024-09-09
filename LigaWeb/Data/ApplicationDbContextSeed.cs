using LigaWeb.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace LigaWeb.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            // Obtém o gerenciador de roles e usuários através do DI
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            // Verifica se a role 'Admin' já existe, se não, cria a role
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Verifica se o usuário administrador já existe
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
            if (adminUser == null)
            {
                // Cria o usuário administrador
                var user = new User
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                };

                // Define a senha do usuário
                var result = await userManager.CreateAsync(user, "Admin123!");

                // Se o usuário foi criado com sucesso, atribui a role de Admin
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
