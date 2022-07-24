using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static AllupBackendProject.Helpers.Helper;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AllupBackendProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new AppUser
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.Username,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }


            await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());

            return RedirectToAction("register", "account");
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM login, string ReturnUrl)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = await _userManager.FindByEmailAsync(login.Email);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or password Invalid.");
                return View(login);
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(appUser, login.Password, true, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account is Blocked");
                return View(login);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password Invalid.");
                return View(login);
            }

            await _signInManager.SignInAsync(appUser, isPersistent: true);

            var roles = await _userManager.GetRolesAsync(appUser);

            foreach (var item in roles)
            {
                if (item == "Admin")
                {
                    return RedirectToAction("index", "dashboard", new { area = "AdminPanel" });
                }
            }

            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }




            return RedirectToAction("index", "home");
        }







        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }











        public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            }
        }
    }
}
