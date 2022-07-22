using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
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
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = await _context.Sliders.ToListAsync();
            homeVM.Categories = await _context.Categories.ToListAsync();
            homeVM.Blogs = await _context.Blogs.ToListAsync();
            homeVM.Banners = await _context.Banners.ToListAsync();
            homeVM.Products = await _context.Products.ToListAsync();
            

            return View(await Task.FromResult(homeVM)); 
        }
    }
}
