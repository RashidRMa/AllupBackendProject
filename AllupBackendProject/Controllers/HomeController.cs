using AllupBackendProject.DAL;
using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

            List<Product> products = _context.Products.Include(p => p.ProductImages).ToList();
            List<Category> categories = await _context.Categories.ToListAsync();


            Category MobilephoneTab = categories.FirstOrDefault(x => x.Id == 1);
            Category TVAudioVideo = categories.FirstOrDefault(x => x.Id == 11);
            Category ComputerAcc = categories.FirstOrDefault(x => x.Id == 18);

            List<Category> mobilephoneTabs = categories.Where(c => c.ParentId == MobilephoneTab.Id).ToList();
            List<Category> tvAudioVideos = categories.Where(c => c.ParentId == TVAudioVideo.Id).ToList();
            List<Category> computerAccs = categories.Where(c => c.ParentId == ComputerAcc.Id).ToList();



            HomeVM homeVM = new HomeVM();

            if (mobilephoneTabs.Count == 0)
            {
                homeVM.MobilephoneTabs = products.Where(p => p.CategoryId == MobilephoneTab.Id).ToList();
            }
            else
            {
                foreach (var item in mobilephoneTabs)
                {
                    homeVM.MobilephoneTabs = products.Where(p => p.CategoryId == item.Id).ToList();
                }
            }

            if (tvAudioVideos.Count == 0)
            {
                homeVM.TVAudioVideos = products.Where(p => p.CategoryId == TVAudioVideo.Id).ToList();
            }
            else
            {
                foreach (var item in tvAudioVideos)
                {
                    homeVM.TVAudioVideos = products.Where(p => p.CategoryId == item.Id).ToList();
                }
            }

            if (computerAccs.Count == 0)
            {
                homeVM.Computers = products.Where(p => p.CategoryId == ComputerAcc.Id).ToList();
            }
            else
            {
                foreach (var item in computerAccs)
                {
                    homeVM.Computers = products.Where(p => p.CategoryId == item.Id).ToList();
                }
            }

            homeVM.Sliders = await _context.Sliders.ToListAsync();
            homeVM.Categories = categories;
            homeVM.Blogs = await _context.Blogs.ToListAsync();
            homeVM.Banners = await _context.Banners.Take(2).ToListAsync();
            homeVM.Products = await _context.Products.Include(x=> x.ProductImages).ToListAsync();
            homeVM.Testimonials = await _context.Testimonials.ToListAsync();
            homeVM.IsFeatured = products.Where(x => x.IsFeatured == true).ToList();
            homeVM.NewArrival = products.Where(x => x.NewArrival == true).ToList();
            homeVM.BestSeller = products.Where(x => x.BestSeller == true).ToList();


            CategorySubChecker(categories);
            return View(homeVM);
        }


        public IActionResult SearchProduct(string search)
        {
            List<Product> products = _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Id)
                .Where(p => p.Name.ToLower()
                .Contains(search.ToLower()))
                .Take(10)
                .ToList();

            return PartialView("_SearchPartial", products);
        }


        public IActionResult SearchProduct(string search, int id)
        {
            List<Product> products = _context.Products
                .Include(p => p.Category)
                .Where(p=> p.CategoryId == id)
                .OrderBy(p => p.Id)
                .Where(p => p.Name.ToLower()
                .Contains(search.ToLower()))
                .Take(10)
                .ToList();

            return PartialView("_SearchPartial", products);
        }









        public void CategorySubChecker(List<Category> categories)
        {
            foreach (var item in categories)
            {
                if (item.ParentId != null)
                {
                    Category category = categories.Find(c => c.Id == item.ParentId);
                    category.Children = new List<Category>();
                    category.Children.Add(item);
                }
            }
        }
    }
}
