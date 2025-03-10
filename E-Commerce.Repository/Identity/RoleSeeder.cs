using Ecommerce.Core.Common.Constants;
using Ecommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Repository.Identity
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAndAdminsAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!await roleManager.RoleExistsAsync(AuthorizationConstants.AdminRole))
            {
                await roleManager.CreateAsync(new UserRole(AuthorizationConstants.AdminRole));
            }

            if (!await roleManager.RoleExistsAsync(AuthorizationConstants.CustomerRole))
            {
                await roleManager.CreateAsync(new UserRole(AuthorizationConstants.CustomerRole));
            }

            var adminUsers = new List<(string Email, string Password)>
            {
                ("etoqa44@gmail.com", "Toqa@123"),
                ("mayarfawzym28@gmail.com", "Mayar28&10#")
            };

            foreach (var (email, password) in adminUsers)
            {
                var adminUser = await userManager.FindByEmailAsync(email);
                if (adminUser == null)
                {
                    adminUser = new AppUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, AuthorizationConstants.AdminRole);
                    }
                }
            }
        }
    }
}