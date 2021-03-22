using FormsWeb.Server.Models;
using FormsWeb.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormsWeb.Server.Data
{
    public static class AuthDBContextInitializer
    {
        public const string DefaultAdminEmailId = "admin@gmail.com";
        public async static Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            List<string> allRoles = new List<string> { Roles.SuperAdmin.ToString(), Roles.Administrator.ToString(), Roles.User.ToString(), Roles.Developer.ToString() };
            foreach (var role in allRoles)
            {
                await CreateRole(roleManager, role);
            }

            string superAdminPassword = configuration.GetValue<string>("SuperAdminPassword");
            await CreateUser(userManager, DefaultAdminEmailId, superAdminPassword, allRoles);
        }

        private async static Task CreateUser(UserManager<ApplicationUser> userManager, string defaultAdminEmailId, string password, List<string> roles)
        {
            if (await userManager.FindByEmailAsync(DefaultAdminEmailId) == null)
            {
                ApplicationUser user = new ApplicationUser() { Email = defaultAdminEmailId, EmailConfirmed = true, UserName = defaultAdminEmailId };
                var userCreationResult = await userManager.CreateAsync(user, password);

                if (userCreationResult.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, roles);
                }
            }
        }

        private async static Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                IdentityRole role = new IdentityRole() { Name = roleName, NormalizedName = roleName };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
