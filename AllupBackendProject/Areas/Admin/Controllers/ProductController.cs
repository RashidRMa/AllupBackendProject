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
                Desc = product.Desc,
                ProductCode = RandomString(5),

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

        public IActionResult Update(int? id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Qwe = new SelectList(_context.Brands.ToList(), "Id", "Name");
            Product product = _context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product product)
        {
            Product dbProduct = await _context.Products.FindAsync(product.Id);
            ProductImage dbProductImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == dbProduct.Id);

            if(dbProduct == null) return NotFound();

            var result = await _photoService.DeletePhotoAsync(dbProductImage.PublicId);

            var newResult = await _photoService.AddPhotoAsync(product.Image);

            if (result.Error != null) return BadRequest(result.Error.Message);
            if (newResult.Error != null) return BadRequest(newResult.Error.Message);

            
            

            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.StockCount = product.StockCount;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.BrandId = product.BrandId;
            dbProduct.ProductImages = product.ProductImages;
            dbProduct.UptadetAt = DateTime.Now;
            dbProduct.Desc = product.Desc;
            if (dbProduct.StockCount >0)
            {
                dbProduct.InStock = true;
            }
            else
            {
                dbProduct.InStock = false;
            }
            



            List<ProductImage> productImages = new List<ProductImage>();
            dbProductImage.ImageUrl = newResult.SecureUrl.AbsoluteUri;
            dbProductImage.PublicId = dbProductImage.PublicId;
            dbProductImage.ProductId = dbProduct.Id;
            dbProductImage.IsMain = true;

            productImages.Add(dbProductImage);
            dbProduct.ProductImages = productImages;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
