using AllupBackendProject.DAL;
using AllupBackendProject.Extentions;
using AllupBackendProject.Helpers;
using AllupBackendProject.Interfaces;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
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
            
            return View(pagination);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            //ViewBag.Brand = _context.Brands.ToListAsync();
            ViewBag.Qwe = new SelectList(_context.Brands.ToList(), "Id", "Name");
            Product product = new Product();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            
            if (!ModelState.IsValid) return View();

            var result = await _photoService.AddPhotoAsync(product.Image);

            if (result.Error != null) return BadRequest(result.Error.Message);

            Product newProduct = new Product
            {
                Name = product.Name,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                ProductImages = product.ProductImages,
                CreatedAt = DateTime.Now,
                Price = product.Price,
                StockCount = product.StockCount,

            };
            List<ProductImage> productImages = new List<ProductImage>();
            ProductImage newproductImage = new ProductImage
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                ProductId = newProduct.Id,
                IsMain = true
            };

            productImages.Add(newproductImage);
            newProduct.ProductImages = productImages;
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            Product dbProduct = await _context.Products.FindAsync(id);

            if (dbProduct == null) return NotFound();

            ProductImage dbProducImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == dbProduct.Id);

            var result = await _photoService.DeletePhotoAsync(dbProducImage.PublicId);

             _context.Products.Remove(dbProduct);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
    }
}
