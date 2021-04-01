using FormsWeb.Server.Models;
using FormsWeb.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormsWeb.Server.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private UserManager<ApplicationUser> _UserManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;
        }

        [HttpGet]
        public async IAsyncEnumerable<ApplicationUserShared> Get()
        {
            foreach (var user in await _UserManager.Users.ToListAsync())
            {
                var roles = await _UserManager.GetRolesAsync(user);

                yield return new ApplicationUserShared() { EmailId = user.Email, Roles = roles };
            }
        }

        [HttpPost]
        public async Task Post(ApplicationUserShared user)
        {
            var userMatched = await _UserManager.FindByEmailAsync(user.EmailId);

            var roles = await _UserManager.GetRolesAsync(userMatched);

            var newRoles = user.Roles.Where(s => !roles.Contains(s));
            var removedRoles = roles.Where(s => !user.Roles.Contains(s));

            await _UserManager.AddToRolesAsync(userMatched, newRoles);
            
            await _UserManager.RemoveFromRolesAsync(userMatched, removedRoles);
        }

        [HttpPost(nameof(DeleteUser))]
        public async Task DeleteUser(ApplicationUserShared user)
        {
            var userMatched = await _UserManager.FindByEmailAsync(user.EmailId);

            await _UserManager.DeleteAsync(userMatched);
        }
    }
}
