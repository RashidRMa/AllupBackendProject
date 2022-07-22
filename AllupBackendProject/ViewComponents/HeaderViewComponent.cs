using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AllupBackendProject.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            Bio bio = _context.Bios.FirstOrDefault();

            return View(await Task.FromResult(bio)); 
        }
    }
}
