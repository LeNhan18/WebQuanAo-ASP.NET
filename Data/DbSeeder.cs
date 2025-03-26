using Microsoft.AspNetCore.Identity;
using Productt.Constants;
using Productt.Models;

namespace Productt.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // Tạo roles nếu chưa tồn tại
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            await roleManager.CreateAsync(new IdentityRole(Roles.User));

            // Tạo tài khoản Admin
            var admin = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                FullName = "Administrator",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(admin.Email) == null)
            {
                await userManager.CreateAsync(admin, "Admin@123"); // Mật khẩu mặc định
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }
    }
} 