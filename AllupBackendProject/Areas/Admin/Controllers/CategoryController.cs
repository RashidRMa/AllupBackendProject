using AllupBackendProject.DAL;
using AllupBackendProject.Interfaces;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupBackendProject.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;


        public CategoryController(AppDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }



        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            Category category = _context.Categories.FirstOrDefault();
            return View(category);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View();

            //bool existNameCat = _context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower());

            //if (existNameCat)
            //{
            //    ModelState.AddModelError("Name", "Category name is exist!");
            //    return View();
            //}


            var result = await _photoService.AddPhotoAsync(category.Image);

            if (result.Error != null) return BadRequest(result.Error.Message);

            Category newCategory = new Category
            {
                Name = category.Name,
                CreatedAt = DateTime.Now,
                ImageUrl = result.SecureUrl.AbsoluteUri,
                ParentId = category.ParentId
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();


            return RedirectToAction("Index"); 
        }


    }
}
