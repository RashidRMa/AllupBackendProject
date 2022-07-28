using AllupBackendProject.DAL;
using AllupBackendProject.Helpers;
using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            //if (review.SenderName == null || review.SenderEmail == null) return View();

            
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            AppUser user = await _userManager.FindByIdAsync(currentUserId);

            Review newReview = new Review();


            if (User.Identity.IsAuthenticated)
            {
                newReview.SenderEmail = user.Email;
                newReview.SenderName = user.FirstName + " " + user.LastName;
                newReview.SendText = review.SendText;
                newReview.ProductId = productId;
            }
            else
            {
                newReview.SenderName=review.SenderName;
                newReview.SendText = review.SendText;
                newReview.SenderEmail = review.SenderEmail;
                newReview.ProductId = productId;
            }

            

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
