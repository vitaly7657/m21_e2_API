using Microsoft.AspNetCore.Identity;

namespace m21_e2_API.Models
{
    public class RoleInitializer
    {
        //создание статичного пользователя admin и запись его в БД
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminLogin = "admin";
            string password = "Aaaa11!!";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await userManager.FindByNameAsync(adminLogin) == null)
            {
                User admin = new User
                {
                    UserName = adminLogin
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
