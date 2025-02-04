using CrudPlay.Core.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CrudPlay.Infrastructure.Persistance.Identity;

public static class Seed
{
    private static readonly string AdminEmail = "birkan@todo.play";
    private static readonly string AdminPassword = "Password1!";

    public static async Task AddRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[] { RoleConstants.Admin, RoleConstants.User };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminUser = await userManager.FindByEmailAsync(AdminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser { UserName = AdminEmail, Email = AdminEmail, EmailConfirmed = true };
            await userManager.CreateAsync(adminUser, AdminPassword);
            await userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
        }
    }
}
