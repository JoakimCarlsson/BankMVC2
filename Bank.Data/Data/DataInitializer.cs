using Microsoft.AspNetCore.Identity;

namespace Bank.Data.Data
{
    public class DataInitializer
    {
        public static void SeedData(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            AddNewRole(roleManager, "Admin");
            AddNewRole(roleManager, "Cashier");
        }

        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            AddNewUser(userManager, "stefan.holmberg@systementor.se", "Hejsan123#", "Admin");
            AddNewUser(userManager, "stefan.holmberg@nackademin.se", "Hejsan123#", "Cashier");
        }

        private static void AddNewUser(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            if (userManager.FindByEmailAsync(email).Result != null) return;

            IdentityUser user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            IdentityResult result = userManager.CreateAsync(user, password).Result;

            if (result.Succeeded)
                userManager.AddToRoleAsync(user, role).Wait();
        }

        private static void AddNewRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (roleManager.RoleExistsAsync(roleName).Result) return;
            IdentityRole role = new IdentityRole { Name = roleName };
            IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        }
    }
}
