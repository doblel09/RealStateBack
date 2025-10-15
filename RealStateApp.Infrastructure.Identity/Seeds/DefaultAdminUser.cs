using Microsoft.AspNetCore.Identity;
using RealStateApp.Infrastructure.Identity.Entities;


namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser defaultUser = new()
            {
                UserName = "adminuser",
                Email = "adminuser@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Cedula = "402589788",
                FirstName = "Luis",
                LastName = "Florentino"
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }

        }
    }
}
