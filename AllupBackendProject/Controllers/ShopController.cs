using AllupBackendProject.DAL;
using AllupBackendProject.Helpers;
using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupBackendProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ShopController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            
            //CategorySubChecker(categories);
            ShopVM shopVM = new ShopVM()
            {
                Categories = await _context.Categories.ToListAsync(),
                Products = _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .ToList()
            };

            var pagination = await PagedList<Product>.CreateAsync(shopVM.Products, page, pageSize);
            ViewBag.Category = shopVM.Categories;
            
            return View(pagination);
        }

        public IActionResult Detail(int? id, string name)
        {
            if (id == null)
            {
                return NotFound();
            }

            ShopVM shopVM = new ShopVM();

            Product dbProcduct = _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p=> p.TagProducts)
                    .FirstOrDefault(p => p.Id == id);

            if (dbProcduct == null) return NotFound();

            shopVM.Product = dbProcduct;
            shopVM.Categories = _context.Categories.ToList();
            shopVM.Reviews = _context.Reviews.ToList();
                    


            
            return View(shopVM);

        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int productId, Review review)
        {
            if (review.SenderName == null || review.SenderEmail == null) return View();

            AppUser user = new AppUser();
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return RedirectToAction("login", "account");
            }

            Review newReview = new Review
            {
                SenderEmail = review.SenderEmail,
                SenderName = review.SenderName,
                SendText = review.SendText,
                ProductId = productId,

            };

            await _context.AddAsync(newReview);
            _context.SaveChanges();

            return RedirectToAction("detail", new { id = productId });
        }


        //public void CategorySubChecker(List<Category> categories)
        //{
        //    foreach (var item in categories)
        //    {
        //        if (item.ParentId != null)
        //        {
        //            Category category = categories.Find(c => c.Id == item.ParentId);
        //            category.Children = new List<Category>();
        //            category.Children.Add(item);
        //        }
        //    }
        //}
    }
}
