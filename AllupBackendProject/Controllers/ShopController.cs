using AllupBackendProject.DAL;
using AllupBackendProject.Helpers;
using AllupBackendProject.Models;
using AllupBackendProject.ViewModels;
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

        public ShopController(AppDbContext context)
        {
            _context = context;
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
