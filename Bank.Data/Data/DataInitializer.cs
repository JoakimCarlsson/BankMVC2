using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bank.Data.Data
{
    public class DataInitializer
    {
        public static async Task SeedData(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await applicationDbContext.Database.MigrateAsync().ConfigureAwait(false);
            await SeedRoles(roleManager).ConfigureAwait(false);
            await SeedUsers(userManager).ConfigureAwait(false);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await AddNewRole(roleManager, "Admin").ConfigureAwait(false);
            await AddNewRole(roleManager, "Cashier").ConfigureAwait(false);
        }

        private static async Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            await AddNewUser(userManager, "stefan.holmberg@systementor.se", "Hejsan123#", "Admin").ConfigureAwait(false);
            await AddNewUser(userManager, "stefan.holmberg@nackademin.se", "Hejsan123#", "Cashier").ConfigureAwait(false);
        }

        private static async Task AddNewUser(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email).ConfigureAwait(false) != null) return;

            IdentityUser user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, password).ConfigureAwait(false);

            if (result.Succeeded)
                userManager.AddToRoleAsync(user, role).Wait();
        }

        private static async Task AddNewRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName).ConfigureAwait(false)) return;
            IdentityRole role = new IdentityRole { Name = roleName };
            IdentityResult roleResult = await roleManager.CreateAsync(role).ConfigureAwait(false);
        }
    }
}
