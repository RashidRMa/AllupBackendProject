using AllupBackendProject.DAL;
using AllupBackendProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AllupBackendProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = await _context.Sliders.ToListAsync();
            homeVM.Categories = await _context.Categories.Include(x=>x.Children).ToListAsync();
            homeVM.Blogs = await _context.Blogs.ToListAsync();
            homeVM.Banners = await _context.Banners.ToListAsync();
            homeVM.Products = await _context.Products.Include(x=> x.TagProducts).ToListAsync();
            homeVM.Testimonials = await _context.Testimonials.ToListAsync();

            return View(homeVM);
        }
    }
}
