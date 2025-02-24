using Microsoft.AspNetCore.Identity;

namespace EstateFInder.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(RoleEnum.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleEnum.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleEnum.User.ToString()));
        }
    }
}
