using Microsoft.AspNetCore.Identity;
using RealStateApp.Infrastructure.Identity.Entities;


namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultCustomerUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser defaultUser = new()
            {
                UserName = "customeruser",
                Email = "customeruser@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                FirstName = "Armando",
                LastName = "Perez",
                Cedula = "4513218987"
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Customer.ToString());
                }
            }

        }
    }
}
