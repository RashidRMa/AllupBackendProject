using AllupBackendProject.DAL;
using AllupBackendProject.Helpers;
using AllupBackendProject.Interfaces;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupBackendProject.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IPhotoService photoService, IWebHostEnvironment env)
        {
            _context = context;
            _photoService = photoService;
            _env = env;
        }

        
        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            List<Product> products = await _context.Products.Include(x => x.ProductImages)
                .Include(x => x.Brand)
                .Include(x => x.TagProducts)
                .Include(x=> x.Category)
                .ToListAsync();

            var pagination = await PagedList<Product>.CreateAsync(products, page, pageSize);
            ViewBag.Category = _context.Categories.ToListAsync();

            return View(pagination);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            Product product = _context.Products.Include(x=>x.ProductImages).FirstOrDefault();
            return View(product);
        }
    }
}
