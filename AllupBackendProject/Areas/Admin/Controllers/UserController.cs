using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupBackendProject.Areas.Admin.Controllers
{
    [Area("admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index(string search)
        {
            var users = search == null ? _userManager.Users.ToList() : _userManager.Users
               .Where(user => user.UserName.ToLower().Contains(search.ToLower())).ToList();
            return View(users);
        }

        public async Task<IActionResult> Update(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.ToListAsync();

            RoleVM roleVM = new RoleVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles,
                UserRoles = userRoles,
                UserId = user.Id
            };

            return View(roleVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(List<string> roles, string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);

            if (user.UserName == User.Identity.Name) return RedirectToAction("index");


            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(result);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            return View(user);
        }

    }
}
